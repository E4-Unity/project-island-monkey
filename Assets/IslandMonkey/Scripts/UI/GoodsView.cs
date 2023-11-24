using Assets._0_IslandMonkey.Scripts.Extension;
using TMPro;
using UnityEngine;

namespace IslandMonkey.MVVM
{
	public class GoodsView : MonoBehaviour
	{
		[SerializeField] GoodsViewModel goodsViewModel;
		[SerializeField] TextMeshProUGUI goldText;
		[SerializeField] TextMeshProUGUI bananaText;
		[SerializeField] TextMeshProUGUI clamText;

		void Start()
		{
			if (goodsViewModel)
			{
				goodsViewModel.OnUpdated += OnUpdated_Event;
				Fetch();
			}
		}

		void OnDestroy()
		{
			if(goodsViewModel) goodsViewModel.OnUpdated -= OnUpdated_Event;
		}

		void Fetch()
		{
			OnUpdated_Event(GoodsType.Gold);
			OnUpdated_Event(GoodsType.Banana);
			OnUpdated_Event(GoodsType.Clam);
		}

		void OnUpdated_Event(GoodsType goodsType)
		{
			switch (goodsType)
			{
				case GoodsType.Gold:
					goldText.SetText(goodsViewModel.Gold.FormatLargeNumber());
					break;
				case GoodsType.Banana:
					bananaText.SetText(goodsViewModel.Banana.FormatLargeNumber());
					break;
				case GoodsType.Clam:
					clamText.SetText(goodsViewModel.Clam.FormatLargeNumber());
					break;
			}
		}
	}
}
