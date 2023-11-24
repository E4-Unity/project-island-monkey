using E4.Utility;
using UnityEngine;

namespace IslandMonkey
{
	[RequireComponent(typeof(GoodsManager))]
	public class GlobalGameManager : GenericMonoSingleton<GlobalGameManager>
	{
		GoodsManager goodsManager;

		public GoodsManager GetGoodsManager() => goodsManager;

		protected override void Awake()
		{
			base.Awake();

			if (!ReferenceEquals(Instance, this)) return;

			goodsManager = GetComponent<GoodsManager>();
			DontDestroyOnLoad(gameObject);
		}
	}
}
