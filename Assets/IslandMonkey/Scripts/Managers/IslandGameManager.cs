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
		public MonkeyBank GetMonkeyBank() => GlobalGameManager.Instance.GetMonkeyBank();
		public GoodsManager GetGoodsManager() => GlobalGameManager.Instance.GetGoodsManager();

		protected override void Init()
		{
			base.Init();

			buildingManager = GetComponent<BuildingManager>();
			placementManager = GetComponent<HexagonalPlacementManager>();
		}
	}
}
