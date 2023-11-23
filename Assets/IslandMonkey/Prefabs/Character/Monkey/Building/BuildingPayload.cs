using UnityEngine;

namespace IslandMonkey
{
	public class BuildingPayload : MonoBehaviour, BuildingMonkey.IBuilding
	{
		[SerializeField] Transform entrance;
		[SerializeField] AnimatorOverrideController animatorController;
		[SerializeField] EquipmentComponent.EquipmentSet equipmentSet;

		public Transform Entrance => entrance;

		public AnimatorOverrideController AnimatorController => animatorController;

		public EquipmentComponent.EquipmentSet Equipments => equipmentSet;
	}
}
