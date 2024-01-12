using E4.Utilities;
using UnityEngine;

namespace IslandMonkey
{
	[RequireComponent(typeof(BuildingManager))]
	public class IslandGameManager : MonoSingleton<IslandGameManager>
	{
		/* 컴포넌트 */
		BuildingManager m_BuildingManager;

		/* API */
		public BuildingManager GetBuildingManager() => m_BuildingManager;

		/* MonoSingleton */
		protected override void InitializeComponent()
		{
			base.InitializeComponent();

			// 컴포넌트 할당
			m_BuildingManager = GetComponent<BuildingManager>();
		}

		protected override bool UseDontDestroyOnLoad => false;
	}
}
