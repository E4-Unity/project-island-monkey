using IslandMonkey;

public class GameManager : Singleton<GameManager>
{
	GoodsManager goodsManager;

	void Awake()
	{
		goodsManager = GlobalGameManager.instance.GetGoodsManager();
	}

	public void EarnGoods(GoodsType goodsType, int amount) => goodsManager.EarnGoods(goodsType, in amount);
	public void SpendGoods(GoodsType goodsType, int amount) => goodsManager.SpendGoods(goodsType, in amount);
	public bool CanSpend(GoodsType goodsType, in int amount) => goodsManager.CanSpend(goodsType, in amount);
}
