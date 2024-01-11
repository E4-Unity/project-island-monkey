using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandMonkey
{
	/// <summary>
	/// 건물 배치 기능 담당
	/// </summary>
	public class HexagonalPlacementManager : MonoBehaviour
	{
		/* 컴포넌트 */
		BuildingFactory m_BuildingFactory;

		/* 필드 */
		[Tooltip("칸 간격")] [SerializeField] float baseDistance = 1.75f;

		[SerializeField] NavMeshSurface navMeshSurface;
		[SerializeField] GameObject buildingSlotPrefab; // TODO Building 으로 변경
		[SerializeField] Transform groundSlot;

		// TODO 임시, 나중에 BuildingManager 로 이전
		List<BuildingMonkey.IBuilding> functionalBuildings = new List<BuildingMonkey.IBuilding>();
		public List<BuildingMonkey.IBuilding> FunctionalBuildings => functionalBuildings;

		HexagonalCalculator calculator;
		BuildingManager buildingManager;
		VoyageDataManager voyageDataManager;
		Camera mainCamera;

		int row = 1;
		int maxRow = 3;
		List<int> usedHexIndices = new List<int>();
		List<int> availableHexIndices = new List<int> { 0, 1, 2, 3, 4, 5 };

		/* 프로퍼티 */
		bool IsFull => row > maxRow;
		bool CanSpawn => usedHexIndices.Count < buildingManager.MaxBuildingCounts;

		void Awake()
		{
			calculator = new HexagonalCalculator(baseDistance);

			mainCamera = Camera.main;

			// TODO 작업중
			// Building Factory 생성
			m_BuildingFactory = new BuildingFactory(groundSlot, buildingSlotPrefab.GetComponent<Building>());
		}

		void Start()
		{
			buildingManager = IslandGameManager.Instance.GetBuildingManager();
			voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();

			foreach (var buildingData in buildingManager.BuildingDataList)
			{
				SpawnBuilding(buildingData, true);
			}

			RefreshNavMesh();

			if (voyageDataManager.ShouldBuild)
			{
				var buildingData = buildingManager.GetBuildingData(voyageDataManager.CurrentBuildingData.Definition.ID);
				if (buildingData is null) return;

				buildingData.IsBuildCompleted = true;
				SpawnBuilding(buildingData);

				voyageDataManager.Clear();

				buildingManager.Save(); // TODO 리팩토링 필요
			}
		}

		public void OnTerritoryExpanded() => maxRow++;

		public void SpawnBuilding(BuildingData buildingData, bool disableBuildEffects = false)
		{
			// 유효성 검사
			if (buildingData is null) return;

			// Hex Index 업데이트
			DisableHexIndex(buildingData.HexIndex);

			// 건설이 완료되었는지 확인
			if (!buildingData.IsBuildCompleted) return;

			// Hex Index에 대응하는 좌표 구하기
			Vector2 pos = calculator.GetPosition(buildingData.HexIndex);
			Vector3 spawnPosition = new Vector3(pos.y, 0, pos.x);

			// 이펙트 연출
			if (!disableBuildEffects)
			{
				// TODO 리팩토링
				// 카메라 이동
				Vector3 cameraOffset = new Vector3(0, 6.14f, -11);

				// Hex Index에 대응하는 좌표 구하기
				mainCamera.transform.position = cameraOffset + spawnPosition;

				// TODO 건설 이펙트 스폰
				EffectManager.Instance.PlayEffect(EffectManager.EffectType.BuildEffect, spawnPosition);

				// 네브 메시 굽기
				RefreshNavMesh();
			}

			// 건물 스폰
			var building = m_BuildingFactory.CreateBuilding(buildingData);
			building.transform.position = spawnPosition;
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

			// 건물 데이터 저장
			var newBuildingData = new BuildingData()
			{
				Definition = buildingManager.GetBuildingDefinition(buildingIndex),
				IsBuildCompleted = spawnImmediately,
				HexIndex = hexIndex,
				BuildStartedTime = (int)span.TotalSeconds
			};

			// 건물 데이터 전송
			buildingManager.RegisterBuildingData(newBuildingData);

			if(!newBuildingData.IsBuildCompleted) // 유학 건물(건설 시간 존재)일 경우에만 전송
				voyageDataManager.CurrentBuildingData = newBuildingData;

			// 스폰
			SpawnBuilding(newBuildingData);
		}

		/* Method */
		void RefreshNavMesh() => navMeshSurface.BuildNavMesh();

		void DisableHexIndex(int hexIndex)
		{
			// 이미 제거된 Hex Index 는 무시
			if (!availableHexIndices.Contains(hexIndex)) return;

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
	}
}
