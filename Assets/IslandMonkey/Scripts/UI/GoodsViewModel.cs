using System;
using System.ComponentModel;

namespace IslandMonkey.MVVM
{
	public class GoodsViewModel : ViewModel
	{
		int gold;
		int banana;
		int clam;

		public int Gold
		{
			get => gold;
			private set
			{
				gold = value;
				OnUpdated?.Invoke(GoodsType.Gold);
			}
		}

		public int Banana
		{
			get => banana;
			private set
			{
				banana = value;
				OnUpdated?.Invoke(GoodsType.Banana);
			}
		}

		public int Clam
		{
			get => clam;
			private set
			{
				clam = value;
				OnUpdated?.Invoke(GoodsType.Clam);
			}
		}

		/* 이벤트 */
		public event Action<GoodsType> OnUpdated;

		void Start()
		{
			var model = GlobalGameManager.Instance.GetGoodsManager();
			if (model)
			{
				Init(model);
				Fetch(model);
			}
		}

		void Fetch(object sender)
		{
			Fetch<int>(sender, nameof(Gold));
			Fetch<int>(sender, nameof(Banana));
			Fetch<int>(sender, nameof(Clam));
		}

		protected override void OnPropertyChanged_Event(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "Gold":
					Fetch<int>(sender, nameof(Gold));

					break;
				case "Banana":
					Fetch<int>(sender, nameof(Banana));

					break;
				case "Clam":
					Fetch<int>(sender, nameof(Clam));

					break;
			}
		}
	}
}
