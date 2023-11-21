using IslandMonkey.MVVM;

public class GameManager : Singleton<GameManager>
{
	GoodsModel goodsModel;

	void Awake()
	{
		goodsModel = GetComponentInChildren<GoodsModel>();
	}

	public void EarnGold(int amount) => goodsModel.EarnGold(in amount);
	public void EarnBanana(int amount) => goodsModel.EarnBanana(in amount);
	public void EarnClam(int amount) => goodsModel.EarnClam(in amount);
}
