using System;
using System.Numerics;
using IslandMonkey.MVVM;
using IslandMonkey.Utils;
using UnityEngine;

namespace IslandMonkey
{
	[Serializable]
	public class MonkeyBankSaveData
	{
		public SerializedBigInteger Gold = new SerializedBigInteger();
		public int Level = 1;
		public int LastGetRewardsTime;
	}

	public class MonkeyBank : Model, DataManager.ISavable<MonkeyBankSaveData>
	{
		/* Field */
		[SerializeField] string[] goldLimitList;

		MonkeyBankSaveData saveData = new MonkeyBankSaveData();

		SerializedBigInteger goldLimit = new SerializedBigInteger();

		int maxTimeRecord = 180 * 60;

		/* Property */
		public BigInteger Gold
		{
			get => saveData.Gold.Value;
			private set
			{
				var newCurrentGold = value.Clamp(BigInteger.Zero, GoldLimit);
				SetField(ref saveData.Gold.Value, newCurrentGold);
				DataManager.SaveData(this);
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
			get => saveData.Level;
			private set
			{
				SetField(ref saveData.Level, Mathf.Clamp(value, 1, goldLimitList.Length));
				GoldLimit = goldLimitList[Level - 1].ToBigInteger();
				DataManager.SaveData(this);
			}
		}

		public int LastGetRewardsTime
		{
			get => saveData.LastGetRewardsTime;
			private set
			{
				SetField(ref saveData.LastGetRewardsTime, value);
				DataManager.SaveData(this);
			}
		}

		public int MaxTimeRecord => maxTimeRecord;

		public bool IsFull => saveData.Gold.Value == goldLimit.Value;

		/* MonoBehaviour */
		void Awake()
		{
			var loadData = DataManager.LoadData(this);
			if (loadData is not null)
			{
				saveData = loadData;
			}
			else
			{
				saveData.LastGetRewardsTime = GetCurrentTime();
				DataManager.SaveData(this);
			}

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
			DataManager.SaveData(this);
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

		/* ISavable 인터페이스 */
		public const string SaveFileName = "MonkeyBankSaveData.json";
		public string FileName => SaveFileName;
		public MonkeyBankSaveData Data => saveData;
	}
}
