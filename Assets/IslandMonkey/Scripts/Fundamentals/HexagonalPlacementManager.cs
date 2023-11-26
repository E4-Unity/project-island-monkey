using System;
using System.Collections.Generic;
using IslandMonkey;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HexagonalPlacementManager : MonoBehaviour
{
	[Tooltip("칸 간격")]
	[SerializeField] float baseDistance = 1.75f;

	[SerializeField] GameObject[] buildings;
	[SerializeField] GameObject buildingSlotPrefab;
	[SerializeField] Transform groundSlot;

	HexagonalCalculator calculator;

	int row = 1;
	int maxRow = 3;
	List<GameObject> instances = new List<GameObject>();
	List<int> buildingIndices;

	bool IsFull => row > maxRow;
	bool CanSpawn => true;

	void Awake()
	{
		calculator = new HexagonalCalculator(baseDistance);

		// Index 초기화
		buildingIndices = new List<int>(buildings.Length);
		for (int i = 0; i < buildings.Length; i++)
		{
			buildingIndices.Add(i);
		}
	}

	void Start()
	{
		var buildingManager = IslandGameManager.Instance.GetBuildingManager();
		foreach (var building in buildingManager.Buildings)
		{
			buildingIndices.Remove(building.HexIndex);
		}
	}

	public void OnTerritoryExpanded() => maxRow++;

	public void SpawnBuilding(int buttonIndex, out int hexIndex)
	{
		// 스폰 실패
		hexIndex = -1;

		// 이미 최대 수의 건물이 생성되었다면 더 이상 생성하지 않음
		if (instances.Count >= buildings.Length) return;

		// 사용 가능한 인덱스 리스트에서 랜덤한 인덱스를 선택
		int randomIndex = Random.Range(0, buildingIndices.Count);
		hexIndex = buildingIndices[randomIndex];
		// 선택된 헥사곤 위치의 인덱스
		buildingIndices.RemoveAt(randomIndex);

		// 모든 인덱스가 사용되었다면 새로운 행으로 전환
		if (buildingIndices.Count == 0 && !IsFull)
		{
			row++;
			int start = calculator.SumHexagonalCentredNumbers(row);
			int end = calculator.SumHexagonalCentredNumbers(row + 1);
			for (int i = start; i < end; i++)
			{
				buildingIndices.Add(i);
			}
		}

		// 인덱스에 대응하는 좌표에 스폰
		calculator.GetCoordinates(hexIndex, out var p, out var q, out var r);
		Vector2 pos = calculator.GetPosition(p, q, r);

		GameObject buildingSlot = Instantiate(buildingSlotPrefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
		buildingSlot.transform.parent = groundSlot;

		GameObject buildingInstance = Instantiate(buildings[buttonIndex], buildingSlot.transform);
		instances.Add(buildingInstance);
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

	public Vector3 GetLastSpawnedBuildingPosition()
	{
		if (instances.Count > 0)
		{
			return instances[instances.Count - 1].transform.position;
		}
		return Vector3.zero;
	}

	public GameObject GetBuildingPrefab(int index)
	{
		if (index >= 0 && index < buildings.Length)
		{
			return buildings[index];
		}
		return null;
	}

}
