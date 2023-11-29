using E4.Utility;
using UnityEngine;

namespace IslandMonkey
{
	[RequireComponent(typeof(GoodsManager), typeof(MonkeyBank), typeof(VoyageDataManager))]
	public class GlobalGameManager : GenericMonoSingleton<GlobalGameManager>
	{
		GoodsManager goodsManager;
		MonkeyBank monkeyBank;
		VoyageDataManager voyageDataManager;

		// 레퍼런스가 필요한 클래스의 Start 이벤트 때 할당 가능합니다. Awake 때 접근하면 null입니다.
		public GoodsManager GetGoodsManager() => goodsManager;
		public MonkeyBank GetMonkeyBank() => monkeyBank;
		public VoyageDataManager GetVoyageDataManager() => voyageDataManager;

		protected override void Init()
		{
			base.Init();

			goodsManager = GetComponent<GoodsManager>();
			monkeyBank = GetComponent<MonkeyBank>();
			voyageDataManager = GetComponent<VoyageDataManager>();

			DontDestroyOnLoad(gameObject);
		}
	}
}
