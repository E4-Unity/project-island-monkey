using System;
using System.Numerics;
using IslandMonkey.MVVM;
using UnityEngine;

namespace IslandMonkey
{
	[Serializable]
	public class GoodsSaveData : ISerializationCallbackReceiver
	{
		public BigInteger Gold;
		public BigInteger Banana;
		public BigInteger Clam;

		[SerializeField] string goldString;
		[SerializeField] string bananaString;
		[SerializeField] string clamString;

		public void OnBeforeSerialize()
		{
			goldString = Gold.ToString();
			bananaString = Banana.ToString();
			clamString = Clam.ToString();
		}

		public void OnAfterDeserialize()
		{
			Gold = BigInteger.Parse(goldString);
			Banana = BigInteger.Parse(bananaString);
			Clam = BigInteger.Parse(clamString);
		}
	}
	public enum GoodsType
	{
		None,
		Gold,
		Banana,
		Clam
	}

	/// <summary>
	/// 프로퍼티가 SaveData를 직접 참고하기 때문에 주의해야합니다.
	/// </summary>
	public class GoodsManager : Model, DataManager.ISavable<GoodsSaveData>
	{
		GoodsSaveData goodsSaveData = new GoodsSaveData();

		public BigInteger Gold
		{
			get => goodsSaveData.Gold;
			private set
			{
				SetField(ref goodsSaveData.Gold, BigInteger.Max(BigInteger.Zero, value));
				OnGoodsUpdated?.Invoke(GoodsType.Gold);
				DataManager.SaveData(this);
			}
		}

		public BigInteger Banana
		{
			get => goodsSaveData.Banana;
			private set
			{
				SetField(ref goodsSaveData.Banana, BigInteger.Max(BigInteger.Zero, value));
				OnGoodsUpdated?.Invoke(GoodsType.Banana);
				DataManager.SaveData(this);
			}
		}

		public BigInteger Clam
		{
			get => goodsSaveData.Clam;
			private set
			{
				SetField(ref goodsSaveData.Clam, BigInteger.Max(BigInteger.Zero, value));
				OnGoodsUpdated?.Invoke(GoodsType.Clam);
				DataManager.SaveData(this);
			}
		}

		/* Event */
		public event Action<GoodsType> OnGoodsUpdated;

		void Awake()
		{
			var saveData = DataManager.LoadData(this);
			if (saveData is not null)
				goodsSaveData = saveData;
		}

		public void EarnGoods(GoodsType goodsType, in BigInteger amount)
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

		public void SpendGoods(GoodsType goodsType, in BigInteger amount)
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

		public bool CanSpend(GoodsType goodsType, in BigInteger amount)
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
		public const string SaveFileName = "GoodsSaveData.json";
		public string FileName => SaveFileName;

		public GoodsSaveData Data => goodsSaveData;
	}
}
