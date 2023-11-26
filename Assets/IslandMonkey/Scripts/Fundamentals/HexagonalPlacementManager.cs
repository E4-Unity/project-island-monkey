using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexagonalPlacementManager : MonoBehaviour
{
	[Tooltip("칸 간격")]
	[SerializeField] int _baseDistance = 2;

	[SerializeField] GameObject[] _buildings;

	HexagonalCalculator _calculator;

	int _row = 1;
	int _maxRow = 3;
	List<GameObject> _instances = new List<GameObject>();
	List<int> _indices = new List<int> { 0, 1, 2, 3, 4, 5 };

	bool IsFull => _row > _maxRow;
	bool CanSpawn => true;

	void Awake()
	{
		_calculator = new HexagonalCalculator(_baseDistance);
	}

	public void OnTerritoryExpanded() => _maxRow++;

	public void SpawnBuilding(int index2)
	{
		// 이미 최대 수의 건물이 생성되었다면 더 이상 생성하지 않음
		if (_instances.Count >= _buildings.Length) return;

		// 사용 가능한 인덱스 리스트에서 랜덤한 인덱스를 선택
		int randomIndex = Random.Range(0, _indices.Count);
		int hexIndex = _indices[randomIndex];
		// 선택된 헥사곤 위치의 인덱스
		_indices.RemoveAt(randomIndex);

		// 모든 인덱스가 사용되었다면 새로운 행으로 전환
		if (_indices.Count == 0 && !IsFull)
		{
			_row++;
			int start = _calculator.SumHexagonalCentredNumbers(_row);
			int end = _calculator.SumHexagonalCentredNumbers(_row + 1);
			for (int i = start; i < end; i++)
			{
				_indices.Add(i);
			}
		}

		// 인덱스에 대응하는 좌표에 스폰
		_calculator.GetCoordinates(hexIndex, out var p, out var q, out var r);
		Vector2 pos = _calculator.GetPosition(p, q, r);

		GameObject buildingInstance = Instantiate(_buildings[index2], new Vector3(pos.x, 0, pos.y), Quaternion.identity);
		_instances.Add(buildingInstance);
	}

	[ContextMenu("Test")]
	public void Test()
	{
		for (int i = 0; i < 75; i++)
		{
			_calculator.GetCoordinates(i, out var p, out var q, out var r);
			Vector2 pos = _calculator.GetPosition(p, q, r);
			Instantiate(_buildings[0], new Vector3(pos.x, 0, pos.y), Quaternion.identity);
		}
	}

	public Vector3 GetLastSpawnedBuildingPosition()
	{
		if (_instances.Count > 0)
		{
			return _instances[_instances.Count - 1].transform.position;
		}
		return Vector3.zero;
	}

	public GameObject GetBuildingPrefab(int index)
	{
		if (index >= 0 && index < _buildings.Length)
		{
			return _buildings[index];
		}
		return null;
	}

}
