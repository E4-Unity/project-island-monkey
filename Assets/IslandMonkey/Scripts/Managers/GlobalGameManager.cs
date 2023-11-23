using UnityEngine;

namespace IslandMonkey
{
	[RequireComponent(typeof(GoodsManager))]
	public class GlobalGameManager : Singleton<GlobalGameManager>
	{
		GoodsManager goodsManager;

		public GoodsManager GetGoodsManager() => goodsManager;

		void Awake()
		{
			goodsManager = GetComponent<GoodsManager>();
			DontDestroyOnLoad(gameObject);
		}
	}
}
