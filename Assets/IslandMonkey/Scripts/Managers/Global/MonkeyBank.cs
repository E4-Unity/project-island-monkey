using System;
using System.Numerics;
using E4.Utilities;
using IslandMonkey.Utils;
using UnityEngine;

namespace IslandMonkey
{
	[Serializable]
	public class MonkeyBankSaveData : ISavable
	{
		public SerializedBigInteger Gold = new SerializedBigInteger();
		public int Level = 1;
		public int LastGetRewardsTime;
	}

	public class MonkeyBank : DataManagerClientModel<MonkeyBankSaveData>
	{
		/* Field */
		[SerializeField] string[] goldLimitList;

		SerializedBigInteger goldLimit = new SerializedBigInteger();

		int maxTimeRecord = 180 * 60;

		/* Property */
		public BigInteger Gold
		{
			get => Data.Gold.Value;
			private set
			{
				var newCurrentGold = value.Clamp(BigInteger.Zero, GoldLimit);
				SetField(ref Data.Gold.Value, newCurrentGold);
				SaveData();
			}
		}

		public BigInteger GoldLimit
		{
			get => goldLimit.Value;
			private set
			{
				SetField(ref goldLimit.Value, value);
			}
		}

		public int Level
		{
			get => Data.Level;
			private set
			{
				SetField(ref Data.Level, Mathf.Clamp(value, 1, goldLimitList.Length));
				GoldLimit = goldLimitList[Level - 1].ToBigInteger();
				SaveData();
			}
		}

		public int LastGetRewardsTime
		{
			get => Data.LastGetRewardsTime;
			private set
			{
				SetField(ref Data.LastGetRewardsTime, value);
				SaveData();
			}
		}

		public int MaxTimeRecord => maxTimeRecord;

		public bool IsFull => Data.Gold.Value == goldLimit.Value;

		/* MonoBehaviour */
		protected override void Awake()
		{
			base.Awake();

			// 레벨 초기화
			if (goldLimitList.Length >= Level)
			{
				GoldLimit = goldLimitList[Level - 1].ToBigInteger();
			}
		}

		/* API */
		public void Clear()
		{
			Gold = BigInteger.Zero;
			LastGetRewardsTime = GetCurrentTime();
			SaveData();
		}

		public void LevelUp(int amount = 1)
		{
			if (goldLimitList.Length < Level) return;
			Level += amount;
		}

		public void AddToBank(int amount)
		{
			if (IsFull)
			{
#if UNITY_EDITOR
				Debug.Log("몽키뱅크 꽉 참!");
#endif
				return;
			}

			Gold += amount;
			// TODO UI 업데이트, 사운드 재생 등
		}

		// TODO 라이브러리
		int GetCurrentTime()
		{
			var now = DateTime.Now.ToLocalTime();
			var span = now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
			var currentTime = (int)span.TotalSeconds;

			return currentTime;
		}
	}
}
