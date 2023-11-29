using E4.Utility;
using UnityEngine;

namespace IslandMonkey
{
	[RequireComponent(typeof(BuildingManager), typeof(HexagonalPlacementManager))]
	public class IslandGameManager : GenericMonoSingleton<IslandGameManager>
	{
		BuildingManager buildingManager;
		HexagonalPlacementManager placementManager;

		public BuildingManager GetBuildingManager() => buildingManager;
		public HexagonalPlacementManager GetPlacementManager() => placementManager;

		protected override void Init()
		{
			base.Init();

			buildingManager = GetComponent<BuildingManager>();
			placementManager = GetComponent<HexagonalPlacementManager>();
		}
	}
}
