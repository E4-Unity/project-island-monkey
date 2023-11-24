using E4.Utility;
using IslandMonkey;

public class GameManager : GenericMonoSingleton<GameManager>
{
	GoodsManager goodsManager;

	void Start()
	{
		goodsManager = GlobalGameManager.Instance.GetGoodsManager();
	}

	public void EarnGoods(GoodsType goodsType, int amount) => goodsManager.EarnGoods(goodsType, in amount);
	public void SpendGoods(GoodsType goodsType, int amount) => goodsManager.SpendGoods(goodsType, in amount);
	public bool CanSpend(GoodsType goodsType, in int amount) => goodsManager.CanSpend(goodsType, in amount);
}
