using System.ComponentModel;
using Assets._0_IslandMonkey.Scripts.Extension;
using TMPro;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public class GoodsViewModel : ViewModel
	{
		[SerializeField] TextMeshProUGUI goldText;
		[SerializeField] TextMeshProUGUI bananaText;
		[SerializeField] TextMeshProUGUI clamText;

		void Start()
		{
			var model = GlobalGameManager.Instance.GetGoodsManager();
			Init(model);
		}

		protected override void OnPropertyChanged_Event(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "Gold":
					if (!goldText) return;

					if (GetProperty<int>(sender, e, out var gold))
					{
						goldText.SetText(gold.FormatLargeNumber());
					}
					break;
				case "Banana":
					if (!bananaText) return;

					if (GetProperty<int>(sender, e, out var banana))
					{
						bananaText.SetText(banana.FormatLargeNumber());
					}
					break;
				case "Clam":
					if (!clamText) return;

					if (GetProperty<int>(sender, e, out var clam))
					{
						clamText.SetText(clam.FormatLargeNumber());
					}
					break;
			}
		}
	}
}
