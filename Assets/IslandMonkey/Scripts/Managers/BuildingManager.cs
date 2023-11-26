using System;
using System.Collections.Generic;
using UnityEngine;

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
		public int buildingIndex; // 건물의 인덱스
		public Vector3 position; // 건물의 위치
		public bool isCompleted; // 건물이 완성되었는지 여부
		public int monkeyType; // 몽키 타입 정보
		public bool hasMonkey; // 원숭이 생성 여부를 나타내는 새로운 필드

		public int HexIndex; // 건물 위치 인덱스 (육각 좌표계)
		// 필요한 추가 정보를 여기에 추가
	}

	public class BuildingManager : MonoBehaviour, DataManager.ISavable
	{
		List<BuildingData> buildings; // 건물 정보 리스트
		public List<BuildingData> Buildings => buildings;

		void Awake()
		{
			var data = DataManager.LoadData<SerializableList<BuildingData>>(this);
			buildings = data is null ? new List<BuildingData>() : data.list;
		}

		// TODO 인덱스 캐싱
		public bool IsBuildingAlreadyExist(int index)
		{
			var existingData = buildings.Find(data => data.buildingIndex == index);
			return existingData is not null;
		}

		public void AddBuilding(BuildingData buildingData)
		{
			buildings.Add(buildingData);
			DataManager.SaveData(this);
		}

		/* ISavable 인터페이스 구현 */
		public string FileName => "buildingData.json";
		public object Data => new SerializableList<BuildingData>(buildings);
	}
}
