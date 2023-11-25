using System;
using IslandMonkey.AssetCollections;
using UnityEngine;

namespace IslandMonkey
{
	public class BuildingPayload : MonoBehaviour, BuildingMonkey.IBuilding
	{
		[SerializeField] Transform entrance;
		[SerializeField] AnimatorOverrideController animatorController;
		[SerializeField] EquipmentComponent.EquipmentSet equipmentSet;

		BuildingAnimator buildingAnimator;

		void Awake()
		{
			buildingAnimator = GetComponent<BuildingAnimator>();
		}

		public Transform Entrance => entrance;

		public AnimatorOverrideController AnimatorController => animatorController;

		public EquipmentComponent.EquipmentSet Equipments => equipmentSet;
		public void ToggleAnimation() => buildingAnimator.ToggleAnimation();
	}
}
