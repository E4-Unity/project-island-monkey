using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandMonkey
{
	public class HexagonalPlacementManager : MonoBehaviour
	{
		[Tooltip("칸 간격")] [SerializeField] float baseDistance = 1.75f;

		[SerializeField] GameObject[] buildings;
		[SerializeField] GameObject buildingSlotPrefab;
		[SerializeField] Transform groundSlot;

		HexagonalCalculator calculator;
		BuildingManager buildingManager;
		VoyageDataManager voyageDataManager;

		int row = 1;
		int maxRow = 3;
		List<int> usedHexIndices = new List<int>();
		List<int> availableHexIndices = new List<int> { 0, 1, 2, 3, 4, 5 };

		bool IsFull => row > maxRow;
		bool CanSpawn => usedHexIndices.Count < buildings.Length;

		void Awake()
		{
			calculator = new HexagonalCalculator(baseDistance);
		}

		void Start()
		{
			buildingManager = IslandGameManager.Instance.GetBuildingManager();
			foreach (var buildingData in buildingManager.BuildingDataList)
			{
				SpawnBuilding(buildingData);
			}

			voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();
		}

		public void OnTerritoryExpanded() => maxRow++;

		public void SpawnBuilding(BuildingData buildingData)
		{
			// 유효성 검사
			if (buildingData is null) return;

			// Hex Index 업데이트
			DisableHexIndex(buildingData.HexIndex);

			// 건설이 완료되었는지 확인
			if (!buildingData.IsBuildCompleted) return;

			// 인덱스에 대응하는 좌표에 스폰
			Vector2 pos = calculator.GetPosition(buildingData.HexIndex);

			// Building Slot 스폰
			GameObject buildingSlot =
				Instantiate(buildingSlotPrefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
			buildingSlot.transform.parent = groundSlot;

			// Building 스폰
			GameObject buildingInstance = Instantiate(buildings[buildingData.BuildingIndex], buildingSlot.transform);
		}

		// TODO 반환이 아니라 즉시 BuildingManager 에 추가?
		public void RequestSpawnBuilding(int buildingIndex, bool spawnImmediately = true)
		{
			// 최대 영토 개수 도달
			if (IsFull) return;

			// 모든 건물들이 다 지어짐
			if (!CanSpawn) return;

			// 랜덤 인덱스 뽑기
			int randomIndex = Random.Range(0, availableHexIndices.Count);
			int hexIndex = availableHexIndices[randomIndex];

			// 건설 시작 시간 기록
			var now = DateTime.Now.ToLocalTime();
			var span = now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
			voyageDataManager.BuildingTimeStamp = (int)span.TotalSeconds;

			// 건물 데이터 저장
			var newBuildingData = new BuildingData()
			{
				BuildingIndex = buildingIndex,
				IsBuildCompleted = spawnImmediately,
				HexIndex = hexIndex,
				BuildStartedTime = (int)span.TotalSeconds
			};

			buildingManager.RegisterBuildingData(newBuildingData);
			voyageDataManager.BuildingTimeStamp = newBuildingData.BuildStartedTime;

			// 스폰
			SpawnBuilding(newBuildingData);
		}

		void DisableHexIndex(int hexIndex)
		{
			availableHexIndices.Remove(hexIndex);
			usedHexIndices.Add(hexIndex);

			if (availableHexIndices.Count == 0) // row 꽉 참
			{
				row++;
				if (!IsFull)
				{
					int start = calculator.SumHexagonalCentredNumbers(row);
					int end = calculator.SumHexagonalCentredNumbers(row + 1);
					availableHexIndices = new List<int>(end - start);
					for (int i = start; i < end; i++)
					{
						availableHexIndices.Add(i);
					}
				}
			}
		}

		[ContextMenu("Test")]
		public void Test()
		{
			for (int i = 0; i < 75; i++)
			{
				calculator.GetCoordinates(i, out var p, out var q, out var r);
				Vector2 pos = calculator.GetPosition(p, q, r);
				Instantiate(buildings[0], new Vector3(pos.x, 0, pos.y), Quaternion.identity);
			}
		}

		public Vector3 GetLastSpawnedBuildingPosition() =>
			usedHexIndices.Count != 0
				? calculator.GetPosition(usedHexIndices[^1])
				: Vector3.zero;

		public GameObject GetBuildingPrefab(int index)
		{
			if (index >= 0 && index < buildings.Length)
			{
				return buildings[index];
			}

			return null;
		}

	}
}
