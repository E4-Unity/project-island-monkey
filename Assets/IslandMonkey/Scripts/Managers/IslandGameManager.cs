using E4.Utility;
using UnityEngine;

namespace IslandMonkey
{
	[RequireComponent(typeof(BuildingManager))]
	public class IslandGameManager : GenericMonoSingleton<IslandGameManager>
	{
		BuildingManager buildingManager;

		public BuildingManager GetBuildingManager() => buildingManager;

		protected override void Awake()
		{
			base.Awake();

			buildingManager = GetComponent<BuildingManager>();
		}
	}
}
