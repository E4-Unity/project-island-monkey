using IslandMonkey;
using UnityEngine;

public class GoodsFactory : MonoBehaviour
{
	[SerializeField] GoodsType goodsType;
	[SerializeField] int income = 100;
	protected int Income => income;

	GameManager gameManager;

	void Awake()
	{
		gameManager = GameManager.Instance;
	}

	protected void EarnGoods()
	{
		gameManager.EarnGoods(goodsType, income);
	}
}
