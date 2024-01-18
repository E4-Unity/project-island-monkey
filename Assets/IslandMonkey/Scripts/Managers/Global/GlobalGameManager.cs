using E4.Utilities;
using UnityEngine;

namespace IslandMonkey
{
	/// <summary>
	/// 모든 씬에서 사용하는 컴포넌트들을 모아둔 컨테이너 클래스
	/// </summary>
	[RequireComponent(typeof(GoodsManager), typeof(MonkeyBank), typeof(VoyageDataManager))]
	public class GlobalGameManager : MonoSingleton<GlobalGameManager>
	{
		[RuntimeInitializeOnLoadMethod]
		static void RuntimeInitialize()
		{
#if UNITY_EDITOR
			Application.targetFrameRate = -1;
#elif UNITY_ANDROID
			Application.targetFrameRate = 60;
#endif
		}

		/* 컴포넌트 */
		GoodsManager m_GoodsManager;
		MonkeyBank m_MonkeyBank;
		VoyageDataManager m_VoyageDataManager;

		/* API */
		// TODO 개선
		// 레퍼런스가 필요한 클래스의 Start 이벤트 때 할당 가능합니다. Awake 때 접근하면 null입니다.
		public GoodsManager GetGoodsManager() => m_GoodsManager;
		public MonkeyBank GetMonkeyBank() => m_MonkeyBank;
		public VoyageDataManager GetVoyageDataManager() => m_VoyageDataManager;

		/* MonoSingleton */
		protected override void InitializeComponent()
		{
			base.InitializeComponent();

			// 컴포넌트 할당
			m_GoodsManager = GetComponent<GoodsManager>();
			m_MonkeyBank = GetComponent<MonkeyBank>();
			m_VoyageDataManager = GetComponent<VoyageDataManager>();
		}

		protected override bool UseDontDestroyOnLoad => true;
	}
}
