using IslandMonkey.MVVM;
using UnityEngine;

public class MonkeyBank : Model
{
	public static MonkeyBank Instance { get; private set; } // Singleton 인스턴스
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

	/* MonoBehaviour */
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
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
}
