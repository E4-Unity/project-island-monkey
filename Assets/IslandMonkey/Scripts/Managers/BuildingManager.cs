using System;
using System.Collections.Generic;
using E4.Utilities;
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
	}

	/// <summary>
	/// 생성된 건물 관리
	/// </summary>
	public class BuildingManager : DataManagerClient<BuildingSaveData>
	{
		/* 필드 */
		[SerializeField] BuildingDefinition[] defaultBuildings;

		Dictionary<int, BuildingData> cachedData;
		Dictionary<int, BuildingDefinition> cachedDefinition;
		Dictionary<BuildingType, int> buildingCountByType = new Dictionary<BuildingType, int>();

		[Header("Addressables")]
		[SerializeField] AssetLabelReference m_BuildingDefinitionLabel;
		Dictionary<int, BuildingDefinition> m_BuildingDefinitionDatabase; // Building Definition 데이터베이스

		/* 프로퍼티 */
		public List<BuildingData> BuildingDataList => Data.BuildingDataList;
		public Dictionary<BuildingType, int> BuildingCountByType => buildingCountByType;

		public event Action OnBuildingDataRegistered;

		/* MonoBehaviour */
		protected override void Awake()
		{
			base.Awake();

			// 초기화
			Init();
		}

		/* API */
		public void RegisterBuildingData(BuildingData buildingData)
		{
			if (buildingData is null) return;

			// 데이터 추가 및 캐싱
			Data.BuildingDataList.Add(buildingData);
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

		public int MaxBuildingCounts => m_BuildingDefinitionDatabase.Count;

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
		// TODO Public 해제
		// TODO 임시 사용중
		public void Save() => SaveData();

		/// <summary>
		/// 데이터 로딩 및 BuildingDefinition 검색
		/// </summary>
		void Init()
		{
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
