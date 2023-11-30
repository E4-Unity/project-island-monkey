using System;
using System.Collections.Generic;
using IslandMonkey.MVVM;
using UnityEngine;

namespace IslandMonkey
{
	[Serializable]
	public class BuildingSaveData
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
		public int BuildStartedTime; //

		public bool IsValid => Definition is not null && Definition.ID >= 0;
	}

	/// <summary>
	/// 프로퍼티가 SaveData를 직접 참고하기 때문에 주의해야합니다.
	/// </summary>
	public class BuildingManager : MonoBehaviour, DataManager.ISavable<BuildingSaveData>
	{
		[SerializeField] BuildingDefinition[] defaultBuildings;

		BuildingSaveData buildingSaveData;
		Dictionary<int, BuildingData> cachedData;
		Dictionary<int, BuildingDefinition> cachedDefinition;
		Dictionary<BuildingType, int> buildingCountByType = new Dictionary<BuildingType, int>();

		public List<BuildingData> BuildingDataList => buildingSaveData.BuildingDataList;
		public Dictionary<BuildingType, int> BuildingCountByType => buildingCountByType;

		public event Action OnBuildingDataRegistered;

		void Awake()
		{
			// 저장된 데이터 로드
			var saveData = DataManager.LoadData(this);
			buildingSaveData = saveData ?? new BuildingSaveData();

			// 초기화
			Init();
		}

		public BuildingData GetBuildingData(int index)
		{
			if (IsBuildingExist(index))
			{
				return cachedData[index];
			}

			return null;
		}

		public void Save() // TODO 임시
		{
			DataManager.SaveData(this);
		}

		// TODO 인덱스 캐싱
		public bool IsBuildingExist(int index) => index >= 0 && (cachedData.ContainsKey(index) || cachedDefinition.ContainsKey(index));

		public bool IsBuildingExist(BuildingDefinition definition) =>
			definition is not null && IsBuildingExist(definition.ID);

		public void RegisterBuildingData(BuildingData buildingData)
		{
			if (buildingData is null) return;

			// 데이터 추가 및 캐싱
			buildingSaveData.BuildingDataList.Add(buildingData);
			CachingBuildingData(buildingData);

			// 데이터 저장
			DataManager.SaveData(this);

			// 이벤트 호출
			OnBuildingDataRegistered?.Invoke();
		}

		void Init()
		{
			// 저장된 데이터 처리
			cachedData = new Dictionary<int, BuildingData>(BuildingDataList.Count);
			CachingBuildingDataList(BuildingDataList);

			// 기본 건물 캐싱
			cachedDefinition = new Dictionary<int, BuildingDefinition>(defaultBuildings.Length);
			CachingBuildingDefinitionList(defaultBuildings);
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

		/* ISavable 인터페이스 구현 */
		public const string SaveFileName = "BuildingSaveData.json";
		public string FileName => SaveFileName;
		public BuildingSaveData Data => buildingSaveData;
	}
}
