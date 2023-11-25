using Assets._0_IslandMonkey.Scripts.Extension;
using TMPro;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public class GoodsView : View
	{
		[SerializeField] TextMeshProUGUI goldText;
		[SerializeField] TextMeshProUGUI bananaText;
		[SerializeField] TextMeshProUGUI clamText;

		int gold;
		int banana;
		int clam;

		public int Gold
		{
			set
			{
				if(goldText)
					goldText.SetText(value.FormatLargeNumber());
			}
		}

		public int Banana
		{
			set
			{
				if(bananaText)
					bananaText.SetText(value.FormatLargeNumber());
			}
		}

		public int Clam
		{
			set
			{
				if(clamText)
					clamText.SetText(value.FormatLargeNumber());
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
