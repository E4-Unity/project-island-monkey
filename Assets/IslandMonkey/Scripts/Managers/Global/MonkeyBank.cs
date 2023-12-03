using System.Numerics;
using IslandMonkey.MVVM;
using IslandMonkey.Utils;
using UnityEngine;

public class MonkeyBank : Model
{
	BigInteger gold = 0;
	BigInteger goldLimit = 3000;

	/* 프로퍼티 */
	public BigInteger Gold
	{
		get => gold;
		set
		{
			var newCurrentGold = value.Clamp(BigInteger.Zero, goldLimit);;

			SetField(ref gold, newCurrentGold);
		}
	}

	public bool IsFull => gold == goldLimit;

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
}
