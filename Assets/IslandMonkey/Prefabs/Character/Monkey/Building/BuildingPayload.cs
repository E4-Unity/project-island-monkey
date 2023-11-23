using UnityEngine;

namespace IslandMonkey
{
	public class BuildingPayload : MonoBehaviour, BuildingMonkey.IBuilding
	{
		[SerializeField] Transform entrance;
		[SerializeField] AnimatorOverrideController animatorController;

		public Transform Entrance => entrance;

		public AnimatorOverrideController AnimatorController => animatorController;
	}
}
