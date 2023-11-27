using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandMonkey
{
	[Serializable]
	public class BuildingSaveData
	{
		public List<BuildingData> BuildingDataList;
	}

	// 건물 정보를 저장할 수 있는 직렬화 가능한 클래스
	[Serializable]
	public class BuildingData
	{
		public int BuildingIndex; // 건물의 인덱스 // TODO BuildingDefinition 으로 대체
		public bool IsBuildCompleted; // 건물이 완성되었는지 여부
		public int HexIndex; // 건물 위치 인덱스 (육각 좌표계)
		public int BuildStartedTime; //
	}

	/// <summary>
	/// 프로퍼티가 SaveData를 직접 참고하기 때문에 주의해야합니다.
	/// </summary>
	public class BuildingManager : MonoBehaviour, DataManager.ISavable<BuildingSaveData>
	{
		BuildingSaveData buildingSaveData;
		Dictionary<int, BuildingData> buildingDataBase;

		public List<BuildingData> BuildingDataList => buildingSaveData.BuildingDataList;

		void Awake()
		{
			var saveData = DataManager.LoadData(this);
			if (saveData is null)
			{
				buildingSaveData = new BuildingSaveData();
				buildingDataBase = new Dictionary<int, BuildingData>();
			}
			else
			{
				buildingSaveData = saveData;

				buildingDataBase = new Dictionary<int, BuildingData>(saveData.BuildingDataList.Count);
				foreach (var buildingData in saveData.BuildingDataList)
				{
					buildingDataBase.Add(buildingData.BuildingIndex, buildingData);
				}
			}
		}

		// TODO 인덱스 캐싱
		public bool IsBuildingAlreadyExist(int index) => buildingDataBase.ContainsKey(index);

		public void RegisterBuildingData(BuildingData buildingData)
		{
			buildingSaveData.BuildingDataList.Add(buildingData);
			DataManager.SaveData(this);
		}

		/* ISavable 인터페이스 구현 */
		public string FileName => "buildingData.json";
		public BuildingSaveData Data => buildingSaveData;
	}
}
