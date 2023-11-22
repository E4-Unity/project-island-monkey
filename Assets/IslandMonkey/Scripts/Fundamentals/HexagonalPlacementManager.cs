using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexagonalPlacementManager : MonoBehaviour
{
	[Tooltip("칸 간격")]
	[SerializeField] int _baseDistance = 2;

	[SerializeField] GameObject _prefab;
	[SerializeField] GameObject[] _buildings;

	HexagonalCalculator _calculator;

	int _row = 1;
	int _maxRow = 3;
	List<GameObject> _instances = new List<GameObject>();
	List<int> _indices = new List<int> { 0, 1, 2, 3, 4, 5 };

	bool IsFull => _row > _maxRow;
	//bool CanSpawn => _instances.Count < _buildings.Length;
	bool CanSpawn => true;

	/* MonoBehaviour */
	void Awake()
	{
		_calculator = new HexagonalCalculator(_baseDistance);
	}

	void Start()
	{
		//StartCoroutine(Spawn());
	}

	/* API */
	// 영토 확장
	public void OnTerritoryExpanded() => _maxRow++;

	public void SpawnBuilding()
	{
		if (_instances.Count >= _buildings.Length) return; // 이미 모든 건물이 생성되었으면 더 이상 생성하지 않음

		int randomIndex = Random.Range(0, _indices.Count);
		int index = _indices[randomIndex];
		_indices.RemoveAt(randomIndex);

		_calculator.GetCoordinates(index, out var p, out var q, out var r);
		Vector2 pos = _calculator.GetPosition(p, q, r);

		// _buildings 배열에서 건물 인스턴스 생성
		GameObject buildingInstance = Instantiate(_buildings[_instances.Count], new Vector3(pos.x, 0, pos.y), Quaternion.identity);
		_instances.Add(buildingInstance);

		// _prefab 인스턴스를 생성하고 _buildings 배열의 인스턴스의 자식으로 설정
		GameObject prefabInstance = Instantiate(_prefab, buildingInstance.transform);
		prefabInstance.transform.localPosition = Vector3.zero; // 자식 오브젝트의 상대 위치를 0, 0, 0으로 설정
	}

	IEnumerator Spawn()
	{
		while (true)
		{
			SpawnBuilding();
			yield return new WaitForSeconds(0.5f);
		}
	}

	[ContextMenu("Test")]
	public void Test()
	{
		if (_prefab is null) return;

		for (int i = 0; i < 75; i++)
		{
			_calculator.GetCoordinates(i, out var p, out var q, out var r);
			Vector2 pos = _calculator.GetPosition(p, q, r);
			Instantiate(_prefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
			//print(GetPosition(p, q, r));
		}
	}
}
