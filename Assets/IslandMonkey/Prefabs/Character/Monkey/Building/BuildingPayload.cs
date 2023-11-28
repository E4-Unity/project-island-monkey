using IslandMonkey.AssetCollections;
using UnityEngine;

namespace IslandMonkey
{
	// TODO Building 과 병합?
	public class BuildingPayload : MonoBehaviour, BuildingMonkey.IBuilding
	{
		[SerializeField] Transform entrance;
		[SerializeField] AnimatorOverrideController animatorController;
		[SerializeField] EquipmentComponent.EquipmentSet equipmentSet;
		[SerializeField] MonkeyDefinition monkeyDefinition;
		[SerializeField] BuildingMonkey monkeyPrefab; // TODO 팩토리 패턴 적용

		BuildingMonkey monkey;
		BuildingAnimator buildingAnimator;
		GoodsFactory goodsFactory;

		void Awake()
		{
			buildingAnimator = GetComponent<BuildingAnimator>();
			goodsFactory = GetComponentInParent<GoodsFactory>();
		}

		void Start()
		{
			SpawnBuildingMonkey();
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

		public void Activate()
		{
			buildingAnimator?.Activate();
			goodsFactory?.Activate();
		}

		public void Deactivate()
		{
			buildingAnimator?.Deactivate();
			goodsFactory?.Deactivate();
		}
	}
}
