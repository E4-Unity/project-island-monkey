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
	}

	public class MonkeyBank : Model, DataManager.ISavable<MonkeyBankSaveData>
	{
		/* Field */
		[SerializeField] string[] goldLimitList;

		MonkeyBankSaveData saveData = new MonkeyBankSaveData();

		SerializedBigInteger goldLimit = new SerializedBigInteger();

		/* Property */
		public BigInteger Gold
		{
			get => saveData.Gold.Value;
			set
			{
				var newCurrentGold = value.Clamp(BigInteger.Zero, GoldLimit);
				SetField(ref saveData.Gold.Value, newCurrentGold);
				DataManager.SaveData(this);
			}
		}

		public BigInteger GoldLimit
		{
			get => goldLimit.Value;
			set => goldLimit.Value = value;
		}

		public bool IsFull => saveData.Gold.Value == goldLimit.Value;

		public int Level
		{
			get => saveData.Level;
			set
			{
				saveData.Level = value;
				GoldLimit = goldLimitList[Level - 1].ToBigInteger();
				DataManager.SaveData(this);
			}
		}

		/* MonoBehaviour */
		void Awake()
		{
			var loadData = DataManager.LoadData(this);
			if (loadData is not null)
			{
				saveData = loadData;
			}

			if (goldLimitList.Length >= Level)
			{
				GoldLimit = goldLimitList[Level - 1].ToBigInteger();
			}
		}

		/* API */
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

		/* ISavable 인터페이스 */
		public const string SaveFileName = "MonkeyBankSaveData.json";
		public string FileName => SaveFileName;
		public MonkeyBankSaveData Data => saveData;
	}
}
