using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexagonalPlacementManager : MonoBehaviour
{
	[Tooltip("칸 간격")]
	[SerializeField] private int _baseDistance = 2;

	[SerializeField] private GameObject _prefab;
	[SerializeField] private GameObject[] _buildings;

	private HexagonalCalculator _calculator;

	private int _row = 1;
	private int _maxRow = 3;
	private List<GameObject> _instances = new List<GameObject>();
	private List<int> _indices = new List<int> { 0, 1, 2, 3, 4, 5 };

	private bool IsFull => _row > _maxRow;

	/* MonoBehaviour */
	private void Awake()
	{
		_calculator = new HexagonalCalculator(_baseDistance);
	}

	/* API */
	// 영토 확장
	public void OnTerritoryExpanded()
	{
		_maxRow++;
	}

	public void SpawnBuilding()
	{
		// 최대 영토 개수 도달
		if (IsFull) return;

		// 모든 건물들이 다 지어짐
		if (_instances.Count >= _buildings.Length) return;

		// 랜덤 인덱스 뽑기
		int randomIndex = Random.Range(0, _indices.Count);
		int index = _indices[randomIndex];

		if (_indices.Count == 1) // row 꽉 참
		{
			_row++;
			if (!IsFull)
			{
				int start = _calculator.SumHexagonalCentredNumbers(_row);
				int end = _calculator.SumHexagonalCentredNumbers(_row + 1);
				_indices = new List<int>(end - start);
				for (int i = start; i < end; i++)
				{
					_indices.Add(i);
				}
			}
		}
		else
		{
			_indices.RemoveAt(randomIndex);
		}

		// 인덱스에 대응하는 좌표에 스폰
		_calculator.GetCoordinates(index, out var p, out var q, out var r);
		Vector2 pos = _calculator.GetPosition(p, q, r);
		GameObject instance = Instantiate(_prefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
		_instances.Add(instance);
	}

	[ContextMenu("Test")]
	public void Test()
	{
		if (_prefab == null) return;

		for (int i = 0; i < 75; i++)
		{
			_calculator.GetCoordinates(i, out var p, out var q, out var r);
			Vector2 pos = _calculator.GetPosition(p, q, r);
			Instantiate(_prefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
		}
	}
}
