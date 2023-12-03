using System;
using System.Numerics;
using IslandMonkey.MVVM;
using IslandMonkey.Utils;
using UnityEngine;

namespace IslandMonkey
{
	[Serializable]
	public class MonkeyBankSaveData : ISerializationCallbackReceiver
	{
		public BigInteger Gold;

		[SerializeField] string goldString;

		public void OnBeforeSerialize()
		{
			goldString = Gold.ToString();
		}

		public void OnAfterDeserialize()
		{
			Gold = BigInteger.Parse(goldString);
		}
	}

	public class MonkeyBank : Model, ISerializationCallbackReceiver, DataManager.ISavable<MonkeyBankSaveData>
	{
		MonkeyBankSaveData saveData = new MonkeyBankSaveData();

		BigInteger goldLimit = 3000;

		string goldLimitString;

		/* 프로퍼티 */
		public BigInteger Gold
		{
			get => saveData.Gold;
			set
			{
				var newCurrentGold = value.Clamp(BigInteger.Zero, goldLimit);
				SetField(ref saveData.Gold, newCurrentGold);
				DataManager.SaveData(this);
			}
		}

		public bool IsFull => saveData.Gold == goldLimit;

		/* MonoBehaviour */
		void Awake()
		{
			var loadData = DataManager.LoadData(this);
			if (loadData is not null)
				saveData = loadData;
		}

		/* API */
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

		public void OnBeforeSerialize()
		{
			goldLimitString = goldLimit.ToString();
		}

		public void OnAfterDeserialize()
		{
		}

		/* ISavable 인터페이스 */
		public const string SaveFileName = "MonkeyBankSaveData.json";
		public string FileName => SaveFileName;
		public MonkeyBankSaveData Data => saveData;
	}
}
