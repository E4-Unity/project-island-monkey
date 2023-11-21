using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public class GoodsModel : Model
	{
		int gold;
		int banana;
		int clam;

		public int Gold
		{
			get => gold;
			set => SetField(ref gold, Mathf.Max(0, value));
		}

		public int Banana
		{
			get => banana;
			set => SetField(ref banana, Mathf.Max(0, value));
		}

		public int Clam
		{
			get => clam;
			set => SetField(ref clam, Mathf.Max(0, value));
		}

		public void EarnGold(in int amount)
		{
			Gold += amount;
		}

		public void EarnBanana(in int amount)
		{
			Banana += amount;
		}

		public void EarnClam(in int amount)
		{
			Clam += amount;
		}
	}
}
