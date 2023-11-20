using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public static event Action<int> OnGoldChanged;
	public static event Action<int> OnBananaChanged;
	public static event Action<int> OnClamChanged;

	private int gold;
	private int banana;
	private int clam;

	public int Gold
	{
		get => gold;
		private set
		{
			if (gold != value)
			{
				gold = value;
				OnGoldChanged?.Invoke(gold);
			}
		}
	}

	public int Banana
	{
		get => banana;
		private set
		{
			if (banana != value)
			{
				banana = value;
				OnBananaChanged?.Invoke(banana);
			}
		}
	}

	public int Clam
	{
		get => clam;
		private set
		{
			if (clam != value)
			{
				clam = value;
				OnClamChanged?.Invoke(clam);
			}
		}
	}

	public void EarnGold(int amount)
	{
		Gold += amount;
	}

	public void EarnBanana(int amount)
	{
		Banana += amount;
	}

	public void EarnClam(int amount)
	{
		Clam += amount;
	}
}
