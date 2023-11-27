using System;
using IslandMonkey.MVVM;
using UnityEngine;

namespace IslandMonkey
{
	public class GoodsSaveData
	{
		public int Gold;
		public int Banana;
		public int Clam;
	}
	public enum GoodsType
	{
		None,
		Gold,
		Banana,
		Clam
	}

	public class GoodsManager : Model, DataManager.ISavable<GoodsSaveData>
	{
		GoodsSaveData data = new GoodsSaveData();

		public int Gold
		{
			get => data.Gold;
			private set
			{
				SetField(ref data.Gold, Mathf.Max(0, value));
				DataManager.SaveData(this);
			}
		}

		public int Banana
		{
			get => data.Banana;
			private set
			{
				SetField(ref data.Banana, Mathf.Max(0, value));
				DataManager.SaveData(this);
			}
		}

		public int Clam
		{
			get => data.Clam;
			private set
			{
				SetField(ref data.Clam, Mathf.Max(0, value));
				DataManager.SaveData(this);
			}
		}

		void Awake()
		{
			var saveData = DataManager.LoadData(this);
			if (saveData is not null)
				data = saveData;
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

		/* ISavable */
		public string FileName => "GoodsSaveData.json";

		public GoodsSaveData Data => data;
	}
}
