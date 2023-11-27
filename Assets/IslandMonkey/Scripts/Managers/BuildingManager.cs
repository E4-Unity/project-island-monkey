using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace IslandMonkey
{
	[System.Serializable]
	public class SerializableList<T>
	{
		public List<T> list;
		public SerializableList(List<T> list)
		{
			this.list = list;
		}
	}

	// 건물 정보를 저장할 수 있는 직렬화 가능한 클래스
	[System.Serializable]
	public class BuildingData
	{
		public int BuildingIndex; // 건물의 인덱스 // TODO BuildingDefinition 으로 대체
		public bool IsBuildCompleted; // 건물이 완성되었는지 여부
		public int HexIndex; // 건물 위치 인덱스 (육각 좌표계)
		public int BuildStartedTime; //
	}

	public class BuildingManager : MonoBehaviour, DataManager.ISavable<SerializableList<BuildingData>>
	{
		List<BuildingData> buildingDataList; // 건물 정보 리스트
		public List<BuildingData> BuildingDataList => buildingDataList;

		void Awake()
		{
			var data = DataManager.LoadData(this);
			buildingDataList = data is null ? new List<BuildingData>() : data.list;
		}

		// TODO 인덱스 캐싱
		public bool IsBuildingAlreadyExist(int index)
		{
			var existingData = buildingDataList.Find(data => data.BuildingIndex == index);
			return existingData is not null;
		}

		public void RegisterBuildingData(BuildingData buildingData)
		{
			buildingDataList.Add(buildingData);
			DataManager.SaveData(this);
		}

		/* ISavable 인터페이스 구현 */
		public string FileName => "buildingData.json";
		public SerializableList<BuildingData> Data => new SerializableList<BuildingData>(buildingDataList);
	}
}
