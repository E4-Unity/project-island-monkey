namespace IslandMonkey.MVVM
{
	public class MonkeyBankViewModel : ViewModel
	{
		MonkeyBank monkeyBank;
		GoodsManager goodsManager;

		void Awake()
		{
			// 컴포넌트 할당
			monkeyBank = GlobalGameManager.Instance.GetMonkeyBank();
			goodsManager = GlobalGameManager.Instance.GetGoodsManager();

			// Model 등록
			PropertyNotifier = monkeyBank;
		}

		public void GetRewards()
		{
			goodsManager.EarnGoods(GoodsType.Gold, monkeyBank.Gold);
			monkeyBank.Clear();
		}
	}
}
