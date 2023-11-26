using IslandMonkey.AssetCollections;
using UnityEngine;

namespace IslandMonkey
{
	public class BuildingPayload : MonoBehaviour, BuildingMonkey.IBuilding
	{
		[SerializeField] Transform entrance;
		[SerializeField] AnimatorOverrideController animatorController;
		[SerializeField] EquipmentComponent.EquipmentSet equipmentSet;
		[SerializeField] MonkeyDefinition monkeyDefinition;
		[SerializeField] BuildingMonkey monkeyPrefab; // TODO 팩토리 패턴 적용

		BuildingAnimator buildingAnimator;
		BuildingMonkey monkey;

		void Awake()
		{
			buildingAnimator = GetComponent<BuildingAnimator>();
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
		public void ToggleAnimation() => buildingAnimator.ToggleAnimation();
	}
}
