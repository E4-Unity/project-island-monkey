using System;
using IslandMonkey;
using UnityEngine;

public class GoodsFactory : MonoBehaviour
{
	[SerializeField] GoodsType goodsType;
	[SerializeField] int income = 100;
	protected int Income => income;

	GoodsManager goodsManager;

	protected virtual void Awake()
	{

	}

	protected virtual void Start()
	{
		goodsManager = GlobalGameManager.Instance.GetGoodsManager();
	}

	protected void EarnGoods()
	{
		goodsManager.EarnGoods(goodsType, income);
	}
}
