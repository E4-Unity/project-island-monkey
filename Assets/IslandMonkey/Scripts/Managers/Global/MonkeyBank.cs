using IslandMonkey.MVVM;
using UnityEngine;

public class MonkeyBank : Model
{
	int gold = 0;
	int goldLimit = 3000;

	/* 프로퍼티 */
	public int Gold
	{
		get => gold;
		set
		{
			var newCurrentGold = Mathf.Clamp(value, 0, goldLimit);

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
