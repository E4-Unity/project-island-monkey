using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using IslandMonkey.MVVM;
using IslandMonkey.Utils;

namespace IslandMonkey
{
	[Serializable]
	public class GoodsSaveData
	{
		public SerializedBigInteger Gold = new SerializedBigInteger();
		public SerializedBigInteger Banana = new SerializedBigInteger();
		public SerializedBigInteger Clam = new SerializedBigInteger();
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
		static readonly Dictionary<GoodsType, PropertyInfo> GoodsPropertyInfos = new Dictionary<GoodsType, PropertyInfo>()
		{
			{ GoodsType.Gold, typeof(GoodsManager).GetProperty(nameof(Gold))},
			{ GoodsType.Banana, typeof(GoodsManager).GetProperty(nameof(Banana)) },
			{ GoodsType.Clam, typeof(GoodsManager).GetProperty(nameof(Clam)) }
		};

		public BigInteger Gold
		{
			get => goodsSaveData.Gold.Value;
			private set
			{
				SetField(ref goodsSaveData.Gold.Value, BigInteger.Max(BigInteger.Zero, value));
				OnGoodsUpdated?.Invoke(GoodsType.Gold);
				DataManager.SaveData(this);
			}
		}

		public BigInteger Banana
		{
			get => goodsSaveData.Banana.Value;
			private set
			{
				SetField(ref goodsSaveData.Banana.Value, BigInteger.Max(BigInteger.Zero, value));
				OnGoodsUpdated?.Invoke(GoodsType.Banana);
				DataManager.SaveData(this);
			}
		}

		public BigInteger Clam
		{
			get => goodsSaveData.Clam.Value;
			private set
			{
				SetField(ref goodsSaveData.Clam.Value, BigInteger.Max(BigInteger.Zero, value));
				OnGoodsUpdated?.Invoke(GoodsType.Clam);
				DataManager.SaveData(this);
			}
		}

		/* Event */
		public event Action<GoodsType> OnGoodsUpdated;

		void Awake()
		{
			// 데이터 로드
			var saveData = DataManager.LoadData(this);
			if (saveData is not null)
				goodsSaveData = saveData;
		}

		public void EarnGoods(GoodsType goodsType, in BigInteger amount)
		{
			// 유효성 검사
			if (amount <= 0 || !GoodsPropertyInfos.ContainsKey(goodsType)) return;

			var propertyInfo = GoodsPropertyInfos[goodsType];
			propertyInfo.SetValue(this, (BigInteger)propertyInfo.GetValue(this) + amount);
		}

		public void SpendGoods(GoodsType goodsType, in BigInteger amount)
		{
			if (!CanSpend(goodsType, amount)) return;

			var propertyInfo = GoodsPropertyInfos[goodsType];
			propertyInfo.SetValue(this, (BigInteger)propertyInfo.GetValue(this) - amount);
		}

		public bool CanSpend(GoodsType goodsType, in BigInteger amount)
		{
			// 유효성 검사
			if (amount < 0 || !GoodsPropertyInfos.ContainsKey(goodsType)) return false;

			return (BigInteger)GoodsPropertyInfos[goodsType].GetValue(this) >= amount;
		}

		/* ISavable */
		public const string SaveFileName = "GoodsSaveData.json";
		public string FileName => SaveFileName;

		public GoodsSaveData Data => goodsSaveData;
	}
}
