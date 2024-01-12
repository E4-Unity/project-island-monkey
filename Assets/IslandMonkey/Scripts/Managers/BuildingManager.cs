using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using E4.Utilities;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IslandMonkey
{
	/// <summary>
	/// 건설이 완료되었거나 진행중인 건물 정보 리스트
	/// </summary>
	[Serializable]
	public class BuildingSaveData : ISavable
	{
		public List<BuildingData> BuildingDataList = new List<BuildingData>();
	}

	// 건물 정보를 저장할 수 있는 직렬화 가능한 클래스
	[Serializable]
	public class BuildingData
	{
		public BuildingDefinition Definition;
		public bool IsBuildCompleted; // 건물이 완성되었는지 여부
		public int HexIndex; // 건물 위치 인덱스 (육각 좌표계)
		public int BuildStartedTime;
		public bool ShouldBuild; // 건설 연출 필요

		public bool IsValid => Definition is not null && Definition.ID >= 0;

		/* 생성자 */
		public BuildingData(BuildingDefinition definition)
		{
			if (definition is null) return;

			Definition = definition;
			IsBuildCompleted = definition.BuildingType != BuildingType.Voyage;
			BuildStartedTime = GetCurrentTime();
		}

		/* 메서드 */
		/// <summary>
		/// 현재 시간을 초 단위로 반환
		/// </summary>
		/// <returns>현재 시간</returns>
		int GetCurrentTime()
		{
			// 시작 시간 기록
			var now = DateTime.Now.ToLocalTime();
			var span = now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

			return (int)span.TotalSeconds;
		}
	}

	/// <summary>
	/// 생성된 건물 관리
	/// </summary>
	public class BuildingManager : DataManagerClient<BuildingSaveData>
	{
		/* 필드 */
		// 컴포넌트
		HexagonalPlacementManager m_PlacementManager = new HexagonalPlacementManager();
		VoyageDataManager m_VoyageDataManager;
		BuildingFactory m_BuildingFactory;

		// 설정
		[Header("Default")]
		[SerializeField] BuildingDefinition[] defaultBuildings;

		[Header("Placement")]
		[SerializeField] float m_BaseDistance = 1.25f;

		[Header("Addressables")]
		[SerializeField] AssetLabelReference m_BuildingDefinitionLabel;
		Dictionary<int, BuildingDefinition> m_BuildingDefinitionDatabase; // Building Definition 데이터베이스

		[Header("Building Factory")]
		[SerializeField] Building m_BuildingPrefab;
		[SerializeField] Transform m_GroundSlot;

		[Header("NavMesh")]
		[SerializeField] NavMeshSurface m_NavMeshSurface;

		// 현재 건설된 기능 시설 목록
		List<BuildingMonkey.IBuilding> m_FunctionalBuildings = new List<BuildingMonkey.IBuilding>();
		public ReadOnlyCollection<BuildingMonkey.IBuilding> FunctionalBuildings => m_FunctionalBuildings.AsReadOnly();

		// 캐시
		Dictionary<int, BuildingData> cachedData;
		Dictionary<int, BuildingDefinition> cachedDefinition;
		Dictionary<BuildingType, int> buildingCountByType = new Dictionary<BuildingType, int>();

		/* 프로퍼티 */
		public List<BuildingData> BuildingDataList => Data.BuildingDataList;
		public Dictionary<BuildingType, int> BuildingCountByType => buildingCountByType;

		public event Action OnBuildingDataRegistered;

		/* MonoBehaviour */
		protected override void Awake()
		{
			base.Awake();

			// 컴포넌트 할당
			m_VoyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();
			m_BuildingFactory = new BuildingFactory(m_GroundSlot, m_BuildingPrefab);

			// 초기화
			Init();
		}

		void Start()
		{
			// 이미 건설이 완료된 건물 건설
			foreach (var buildingData in BuildingDataList)
			{
				SpawnBuilding(buildingData);
			}

			BakeNavMesh();

			// 유학 건물 건설 완료 여부 확인
			if (m_VoyageDataManager.ShouldBuild)
			{
				var buildingData = GetBuildingData(m_VoyageDataManager.CurrentBuildingData.Definition.ID);
				if (buildingData is null) return;

				buildingData.IsBuildCompleted = true;
				BuildBuilding(buildingData);

				m_VoyageDataManager.Clear();
			}
		}

		/* API */
		/// <summary>
		/// 유학 시설은 Building Data 만 생성하여 저장하고, 기능, 특별 시설은 건설까지 진행
		/// </summary>
		/// <param name="buildingID"></param>
		public void RequestSpawnBuilding(int buildingID)
		{
			// 최대 영토 개수 도달
			if (m_PlacementManager.IsFull) return;

			// 건물 데이터 생성
			var newBuildingData = new BuildingData(GetBuildingDefinition(buildingID))
			{
				HexIndex = m_PlacementManager.GetRandomHexIndex()
			};

			// TODO Building Manager 이전
			// 건물 데이터 전송
			RegisterBuildingData(newBuildingData);

			// 스폰
			if(newBuildingData.IsBuildCompleted) BuildBuilding(newBuildingData);
			else m_VoyageDataManager.CurrentBuildingData = newBuildingData;
		}

		/// <summary>
		/// 처음으로 건설할 때 사용하는 메서드로 카메라 이동, 건설 이펙트 스폰 등이 포함됨
		/// </summary>
		/// <param name="buildingData">건설할 건물 정보</param>
		public void BuildBuilding(BuildingData buildingData) => SpawnBuilding(buildingData, true);

		/// <summary>
		/// 건물 생성 및 배치
		/// </summary>
		/// <param name="buildingData">건설할 건물 정보</param>
		/// <param name="isFirstBuild">true 시 건설 연출 실행</param>
		public void SpawnBuilding(BuildingData buildingData, bool isFirstBuild = false)
		{
			// 유효성 검사
			if (buildingData is null || !buildingData.IsBuildCompleted) return;

			// 건물 생성 및 배치
			var building = m_BuildingFactory.CreateBuilding(buildingData);
			m_PlacementManager.TryPlaceBuilding(building, buildingData.HexIndex, out var spawnPosition);

			// 기능 건물 등록
			if (buildingData.Definition.BuildingType == BuildingType.Functional)
			{
				// TODO 리팩토링
				BuildingMonkey.IBuilding buildingPayload = building.GetBuildingModel().GetBuildingPayload();
				m_FunctionalBuildings.Add(buildingPayload);
			}

			// 처음 건설되는 경우 건설 연출 실행
			if (!isFirstBuild) return;

			// 건설 정보 저장
			SaveData();

			// 카메라 이동
			Vector3 cameraOffset = new Vector3(0, 6.14f, -11);
			Camera.main.transform.position = cameraOffset + spawnPosition;

			// 건설 이펙트 스폰
			EffectManager.Instance.PlayEffect(EffectManager.EffectType.BuildEffect, spawnPosition);

			// NavMesh 굽기
			BakeNavMesh();
		}

		public void RegisterBuildingData(BuildingData buildingData)
		{
			if (buildingData is null) return;

			// 데이터 추가 및 캐싱
			BuildingDataList.Add(buildingData);
			CachingBuildingData(buildingData);

			// 데이터 저장
			SaveData();

			// 이벤트 호출
			OnBuildingDataRegistered?.Invoke();
		}

		// 쿼리
		public BuildingDefinition GetBuildingDefinition(int id) => m_BuildingDefinitionDatabase.ContainsKey(id)
			? m_BuildingDefinitionDatabase[id]
			: null;

		public BuildingData GetBuildingData(int index)
		{
			if (IsBuildingExist(index))
			{
				return cachedData[index];
			}

			return null;
		}

		// TODO 인덱스 캐싱
		public bool IsBuildingExist(int index) => index >= 0 && (cachedData.ContainsKey(index) || cachedDefinition.ContainsKey(index));

		public bool IsBuildingExist(BuildingDefinition definition) =>
			definition is not null && IsBuildingExist(definition.ID);

		/* 메서드 */
		/// <summary>
		/// 데이터 로딩 및 BuildingDefinition 색인
		/// </summary>
		void Init()
		{
			// Placement Manager 초기화
			InitPlacementManager();

			// 저장된 데이터 처리
			cachedData = new Dictionary<int, BuildingData>(BuildingDataList.Count);
			CachingBuildingDataList(BuildingDataList);

			// 기본 건물 캐싱
			cachedDefinition = new Dictionary<int, BuildingDefinition>(defaultBuildings.Length);
			CachingBuildingDefinitionList(defaultBuildings);

			// 모든 Building Definition 색인
			LoadAllDefinitions();
		}

		/// <summary>
		/// Placement Manager 초기화
		/// </summary>
		void InitPlacementManager()
		{
			m_PlacementManager.BaseDistance = m_BaseDistance;
		}

		/// <summary>
		/// 모든 Building Definition 색인
		/// </summary>
		void LoadAllDefinitions()
		{
			// Building Definition 로드
			var loadingTasks = Addressables.LoadAssetsAsync<BuildingDefinition>(m_BuildingDefinitionLabel.labelString, null);
			var buildingDefinitions = loadingTasks.WaitForCompletion();

			// Building Database 초기화 및 색인
			m_BuildingDefinitionDatabase = new Dictionary<int, BuildingDefinition>(buildingDefinitions.Count);
			foreach (var definition in buildingDefinitions)
			{
				m_BuildingDefinitionDatabase.Add(definition.ID, definition);
			}
		}

		/// <summary>
		/// 런타임 NavMesh 굽기
		/// </summary>
		public void BakeNavMesh() => m_NavMeshSurface.BuildNavMesh();

		void CachingBuildingDataList(IEnumerable<BuildingData> dataList)
		{
			foreach (var data in dataList)
			{
				CachingBuildingData(data);
			}
		}

		void CachingBuildingData(BuildingData data)
		{
			if (data is null || !data.IsValid) return;

			cachedData.Add(data.Definition.ID, data);
			CountBuildingByType(data.Definition);
		}

		void CachingBuildingDefinitionList(IEnumerable<BuildingDefinition> definitionList)
		{
			foreach (var definition in definitionList)
			{
				CachingBuildingDefinition(definition);
			}
		}

		void CachingBuildingDefinition(BuildingDefinition definition)
		{
			if (definition is null) return;

			cachedDefinition.Add(definition.ID, definition);
			CountBuildingByType(definition);
		}

		void CountBuildingByType(BuildingDefinition definition)
		{
			if (definition is null) return;

			if (buildingCountByType.TryGetValue(definition.BuildingType, out var count))
			{
				buildingCountByType.Remove(definition.BuildingType);
				buildingCountByType.Add(definition.BuildingType, count + 1);
			}
			else
			{
				buildingCountByType.Add(definition.BuildingType, 1);
			}
		}
	}
}
