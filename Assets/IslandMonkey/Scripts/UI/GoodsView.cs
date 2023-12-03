using System.Numerics;
using IslandMonkey.Utils;
using TMPro;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public class GoodsView : View
	{
		[SerializeField] TextMeshProUGUI[] goldTextList;
		[SerializeField] TextMeshProUGUI[] bananaTextList;
		[SerializeField] TextMeshProUGUI[] clamTextList;

		public BigInteger Gold
		{
			set
			{
				foreach (var goldText in goldTextList)
				{
					if(goldText)
						goldText.SetText(value.FormatLargeNumber());
				}
			}
		}

		public BigInteger Banana
		{
			set
			{
				foreach (var bananaText in bananaTextList)
				{
					if(bananaText)
						bananaText.SetText(value.FormatLargeNumber());
				}
			}
		}

		public BigInteger Clam
		{
			set
			{
				foreach (var clamText in clamTextList)
				{
					if(clamText)
						clamText.SetText(value.FormatLargeNumber());
				}
			}
		}

		protected override void RegisterProperties()
		{
			RegisterProperty(nameof(Gold));
			RegisterProperty(nameof(Banana));
			RegisterProperty(nameof(Clam));
		}
	}
}
