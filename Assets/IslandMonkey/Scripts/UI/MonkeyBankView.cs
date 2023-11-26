using Assets._0_IslandMonkey.Scripts.Extension;
using TMPro;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public class MonkeyBankView : View
	{
		[SerializeField] TextMeshProUGUI[] goldTextList;

		public int Gold
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

		protected override void RegisterProperties()
		{
			RegisterProperty(nameof(Gold));
		}
	}
}