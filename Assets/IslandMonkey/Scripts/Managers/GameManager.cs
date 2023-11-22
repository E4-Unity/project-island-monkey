using IslandMonkey.MVVM;

public class GameManager : Singleton<GameManager>
{
	GoodsModel goodsModel;

	void Awake()
	{
		goodsModel = GetComponentInChildren<GoodsModel>();
	}

	public void EarnGoods(GoodsType goodsType, int amount) => goodsModel.EarnGoods(goodsType, in amount);
	public void SpendGoods(GoodsType goodsType, int amount) => goodsModel.SpendGoods(goodsType, in amount);
	public bool CanSpend(GoodsType goodsType, in int amount) => goodsModel.CanSpend(goodsType, in amount);
}
