using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using E4.Utilities;
using IslandMonkey.Utils;

namespace IslandMonkey
{
	[Serializable]
	public class GoodsSaveData : ISavable
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
	/// 플레이어의 재화를 관리하는 클래스
	/// </summary>
	public class GoodsManager : DataManagerClientModel<GoodsSaveData>
	{
		/* 필드 */

		static readonly Dictionary<GoodsType, PropertyInfo> m_GoodsPropertyInfos = new Dictionary<GoodsType, PropertyInfo>()
		{
			{ GoodsType.Gold, typeof(GoodsManager).GetProperty(nameof(Gold))},
			{ GoodsType.Banana, typeof(GoodsManager).GetProperty(nameof(Banana)) },
			{ GoodsType.Clam, typeof(GoodsManager).GetProperty(nameof(Clam)) }
		};

		/* 프로퍼티 */

		public BigInteger Gold
		{
			get => Data.Gold.Value;
			private set
			{
				SetField(ref Data.Gold.Value, BigInteger.Max(BigInteger.Zero, value));
				OnGoodsUpdated?.Invoke(GoodsType.Gold);
				SaveData();
			}
		}

		public BigInteger Banana
		{
			get => Data.Banana.Value;
			private set
			{
				SetField(ref Data.Banana.Value, BigInteger.Max(BigInteger.Zero, value));
				OnGoodsUpdated?.Invoke(GoodsType.Banana);
				SaveData();
			}
		}

		public BigInteger Clam
		{
			get => Data.Clam.Value;
			private set
			{
				SetField(ref Data.Clam.Value, BigInteger.Max(BigInteger.Zero, value));
				OnGoodsUpdated?.Invoke(GoodsType.Clam);
				SaveData();
			}
		}

		/* Event */
		public event Action<GoodsType> OnGoodsUpdated;

		public void EarnGoods(GoodsType goodsType, in BigInteger amount)
		{
			// 유효성 검사
			if (amount <= 0 || !m_GoodsPropertyInfos.ContainsKey(goodsType)) return;

			var propertyInfo = m_GoodsPropertyInfos[goodsType];
			propertyInfo.SetValue(this, (BigInteger)propertyInfo.GetValue(this) + amount);
		}

		public void SpendGoods(GoodsType goodsType, in BigInteger amount)
		{
			if (!CanSpend(goodsType, amount)) return;

			var propertyInfo = m_GoodsPropertyInfos[goodsType];
			propertyInfo.SetValue(this, (BigInteger)propertyInfo.GetValue(this) - amount);
		}

		public bool CanSpend(GoodsType goodsType, in BigInteger amount)
		{
			// 유효성 검사
			if (amount < 0 || !m_GoodsPropertyInfos.ContainsKey(goodsType)) return false;

			return (BigInteger)m_GoodsPropertyInfos[goodsType].GetValue(this) >= amount;
		}
	}
}
