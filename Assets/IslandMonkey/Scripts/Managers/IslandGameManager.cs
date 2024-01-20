using E4.Utilities;
using UnityEngine;

namespace IslandMonkey
{
	/// <summary>
	/// 메인 씬에서 사용하는 레퍼런스들을 모아둔 컨테이너 클래스
	/// </summary>
	[RequireComponent(typeof(BuildingManager))]
	public class IslandGameManager : MonoSingleton<IslandGameManager>
	{
		/* 컴포넌트 */
		BuildingManager m_BuildingManager;

		/* 필드 */
		[Header("Config")]
		[SerializeField] ShowcaseMonkey m_ShowcaseMonkey; // 숭숭이 등장 연출용 Monkey 변종 프리팹

		/* API */
		public BuildingManager GetBuildingManager() => m_BuildingManager;
		public ShowcaseMonkey GetShowcaseMonkey() => m_ShowcaseMonkey;

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
