using E4.Utilities;
using UnityEngine;

namespace IslandMonkey
{
	[RequireComponent(typeof(BuildingManager), typeof(HexagonalPlacementManager))]
	public class IslandGameManager : MonoSingleton<IslandGameManager>
	{
		/* 컴포넌트 */
		BuildingManager m_BuildingManager;
		HexagonalPlacementManager m_PlacementManager;

		/* API */
		public BuildingManager GetBuildingManager() => m_BuildingManager;
		public HexagonalPlacementManager GetPlacementManager() => m_PlacementManager;

		/* MonoSingleton */
		protected override void InitializeComponent()
		{
			base.InitializeComponent();

			// 컴포넌트 할당
			m_BuildingManager = GetComponent<BuildingManager>();
			m_PlacementManager = GetComponent<HexagonalPlacementManager>();
		}

		protected override bool UseDontDestroyOnLoad => false;
	}
}
