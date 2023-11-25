namespace IslandMonkey.MVVM
{
	public class GoodsViewModel : ViewModel
	{
		public int Gold { get; set; }
		public int Banana { get; set; }
		public int Clam { get; set; }

		protected override void Start()
		{
			base.Start();

			// Model 등록
			PropertyNotifier = GlobalGameManager.Instance.GetGoodsManager();
		}

		protected override void RegisterProperties()
		{
			RegisterProperty(nameof(Gold));
			RegisterProperty(nameof(Banana));
			RegisterProperty(nameof(Clam));
		}
	}
}
