namespace IslandMonkey.MVVM
{
	public class MonkeyBankViewModel : ViewModel
	{
		public int Gold { get; set; }

		protected override void Start()
		{
			base.Start();

			// Model 등록
			PropertyNotifier = GlobalGameManager.Instance.GetMonkeyBank();
		}

		protected override void RegisterProperties()
		{
			RegisterProperty(nameof(Gold));
		}
	}
}
