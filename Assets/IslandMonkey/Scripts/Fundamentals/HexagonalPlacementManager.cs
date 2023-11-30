using System;
using System.Collections.Generic;
using IslandMonkey.AssetCollections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandMonkey
{
	public class HexagonalPlacementManager : MonoBehaviour
	{
		[Tooltip("칸 간격")] [SerializeField] float baseDistance = 1.75f;

		[SerializeField] BuildingDefinition[] buildings;
		[SerializeField] GameObject buildingSlotPrefab;
		[SerializeField] Transform groundSlot;

		Dictionary<int, BuildingDefinition> buildingDatabase;

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

		bool IsFull => row > maxRow;
		bool CanSpawn => usedHexIndices.Count < buildings.Length;

		void Awake()
		{
			calculator = new HexagonalCalculator(baseDistance);

			// Building Definition 캐싱
			buildingDatabase = new Dictionary<int, BuildingDefinition>(buildings.Length);
			foreach (var building in buildings)
			{
				if(!building || building.ID < 0) continue; // ID 유효성 검사
				buildingDatabase.Add(building.ID, building);
			}

			mainCamera = Camera.main;
		}

		void Start()
		{
			buildingManager = IslandGameManager.Instance.GetBuildingManager();
			voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();

			foreach (var buildingData in buildingManager.BuildingDataList)
			{
				SpawnBuilding(buildingData, true);
			}

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
			Vector3 spawnPosition = new Vector3(pos.x, 0, pos.y);

			if (!disableBuildEffects)
			{
				// TODO 리팩토링
				// 카메라 이동
				Vector3 cameraOffset = new Vector3(0, 6.14f, -11);

				// Hex Index에 대응하는 좌표 구하기
				mainCamera.transform.position = cameraOffset + spawnPosition;

				// TODO 건설 이펙트 스폰
				EffectManager.instance.PlayEffect(EffectManager.EffectType.BuildEffect, spawnPosition);
			}

			// Building Slot 스폰
			if (!buildingSlotPrefab) return;
			GameObject buildingSlot =
				Instantiate(buildingSlotPrefab, spawnPosition, Quaternion.identity);
			buildingSlot.transform.parent = groundSlot;

			// GoodsFactory 초기화
			var goodsFactory = buildingSlot.GetComponentInChildren<GoodsFactory>();
			if (goodsFactory)
			{
				bool activate = buildingData.Definition.BuildingType == BuildingType.Voyage;
				goodsFactory.Init(buildingData.Definition.GetGoodsFactoryConfig(), activate);
			}

			// Building 스폰
			if (!buildingData.Definition.BuildingPrefab) return;
			GameObject buildingInstance = Instantiate(buildingData.Definition.BuildingPrefab, buildingSlot.transform);

			// TODO 리팩토링 필요
			// 특별 건물 활성화
			if (buildingData.Definition.BuildingType == BuildingType.Special)
			{
				BuildingAnimator buildingAnimator = buildingInstance.GetComponent<BuildingAnimator>();
				if (buildingAnimator)
				{
					buildingAnimator.Activate();
				}
			}

			// TODO 리팩토링 필요
			// Building 초기화
			BuildingMonkey.IBuilding building = buildingInstance.GetComponent<BuildingMonkey.IBuilding>();
			if (building is not null)
			{
				building.Init(buildingData);
			}

			// TODO 리팩토링 필요
			// 기능 건물 등록
			if (buildingData.Definition.BuildingType == BuildingType.Functional)
			{
				if (building is not null)
				{
					functionalBuildings.Add(building);
				}
			}
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

			// Building Definition 로드
			if (!buildingDatabase.ContainsKey(buildingIndex))
			{
#if UNITY_EDITOR
				Debug.LogError("BuildingData.BuildingIndex (" + buildingIndex +")에 대응하는 BuildingDefinition 을 찾을 수 없습니다.");
#endif
				return;
			}

			// 건물 데이터 저장
			var newBuildingData = new BuildingData()
			{
				Definition = buildingDatabase[buildingIndex],
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
