using System.Collections;
using IslandMonkeyPack;
using UnityEngine;

namespace IslandMonkey
{
	// TODO Building 과 병합?
	public class BuildingPayload : MonoBehaviour, BuildingMonkey.IBuilding
	{
		/* 필드 */
		[SerializeField] Transform entrance;
		[SerializeField] AnimatorOverrideController animatorController;
		[SerializeField] EquipmentComponent.EquipmentSet equipmentSet;
		[SerializeField] MonkeyDefinition monkeyDefinition;
		[SerializeField] BuildingMonkey monkeyPrefab; // TODO 팩토리 패턴 적용

		bool isActivated;

		// 컴포넌트
		BuildingMonkey monkey;
		BuildingAnimator buildingAnimator;
		GoodsFactory goodsFactory;
		BuildingManager m_BuildingManager;

		// TODO 임시
		BuildingData buildingData;
		float activeTime = 5f;
		float timer = 0;

		void Awake()
		{
			// 컴포넌트 할당
			buildingAnimator = GetComponentInChildren<BuildingAnimator>();
			goodsFactory = GetComponentInParent<GoodsFactory>();
			m_BuildingManager = IslandGameManager.Instance.GetBuildingManager();
		}

		void Start()
		{
			SpawnBuildingMonkey();
		}

		public void Init(BuildingData data)
		{
			if (data is null) return;

			buildingData = data;
			activeTime = buildingData.Definition.ActiveTime;
		}

		// TODO 팩토리 패턴 적용
		void SpawnBuildingMonkey()
		{
			if (!monkeyDefinition || !monkeyPrefab) return;

			monkey = Instantiate(monkeyPrefab, transform);
			monkey.Init(monkeyDefinition);
			monkey.SetBuilding(this);
		}

		/* IBuilding 인터페이스 구현 */
		public Transform Entrance => entrance;

		public AnimatorOverrideController AnimatorController => animatorController;

		public EquipmentComponent.EquipmentSet Equipments => equipmentSet;

		public void Activate(BuildingMonkey inMonkey)
		{
			if (isActivated || inMonkey is null) return;
			isActivated = true;
			IsBusy = false;

			monkey = inMonkey;

			buildingAnimator?.Activate();
			goodsFactory?.Activate();

			if (buildingData is not null && buildingData.Definition.ActiveTime > 0)
			{
				StartCoroutine(CheckActiveTime());
			}
		}

		public void Deactivate()
		{
			if (!isActivated) return;
			isActivated = false;

			buildingAnimator?.Deactivate();
			goodsFactory?.Deactivate();
		}

		public bool IsActivated => isActivated;

		public bool IsBusy { get; set; }

		IEnumerator CheckActiveTime()
		{
			while (timer < activeTime)
			{
				yield return null;
				timer += Time.deltaTime;
			}

			timer = 0;
			if (buildingData.Definition.BuildingType == BuildingType.Functional)
			{
				monkey.BackToWork();
			}
			else if (buildingData.Definition.BuildingType == BuildingType.Voyage)
			{
				while (monkey.State == BuildingMonkey.BuildingMonkeyState.Working)
				{
					foreach (var functionalBuilding in m_BuildingManager.FunctionalBuildings)
					{
						if (!functionalBuilding.IsBusy && !functionalBuilding.IsActivated)
						{
							monkey.GoToRest(functionalBuilding);
							functionalBuilding.IsBusy = true;
							break;
						}
					}
					yield return null;
				}
			}
		}
	}
}
