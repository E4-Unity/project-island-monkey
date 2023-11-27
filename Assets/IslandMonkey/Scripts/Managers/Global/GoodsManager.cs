using IslandMonkey.MVVM;
using UnityEngine;

namespace IslandMonkey
{
	public enum GoodsType
	{
		None,
		Gold,
		Banana,
		Clam
	}

	public class GoodsManager : Model
	{
		int gold;
		int banana;
		int clam;

		public int Gold
		{
			get => gold;
			private set => SetField(ref gold, Mathf.Max(0, value));
		}

		public int Banana
		{
			get => banana;
			private set => SetField(ref banana, Mathf.Max(0, value));
		}

		public int Clam
		{
			get => clam;
			private set => SetField(ref clam, Mathf.Max(0, value));
		}

		public void EarnGoods(GoodsType goodsType, in int amount)
		{
			// 유효성 검사
			if (goodsType == GoodsType.None || amount < 0) return;

			switch (goodsType)
			{
				case GoodsType.Gold:
					Gold += amount;
					break;
				case GoodsType.Banana:
					Banana += amount;
					break;
				case GoodsType.Clam:
					Clam += amount;
					break;
			}
		}

		public void SpendGoods(GoodsType goodsType, in int amount)
		{
			if (!CanSpend(goodsType, amount)) return;

			switch (goodsType)
			{
				case GoodsType.Gold:
					Gold -= amount;
					break;
				case GoodsType.Banana:
					Banana -= amount;
					break;
				case GoodsType.Clam:
					Clam -= amount;
					break;
			}
		}

		public bool CanSpend(GoodsType goodsType, in int amount)
		{
			// 유효성 검사
			if (goodsType == GoodsType.None || amount < 0) return false;

			bool result = false;

			switch (goodsType)
			{
				case GoodsType.Gold:
					if (Gold >= amount) result = true;
					break;
				case GoodsType.Banana:
					if (Banana >= amount) result = true;
					break;
				case GoodsType.Clam:
					if (Clam >= amount) result = true;
					break;
				default:
					result = false;
					break;
			}

			return result;
		}
	}
}
