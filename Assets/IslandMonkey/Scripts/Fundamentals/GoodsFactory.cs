using System;
using IslandMonkey;
using UnityEngine;

public class GoodsFactory : MonoBehaviour
{
	[SerializeField] GoodsType goodsType;
	[SerializeField] int income = 100;
	protected int Income => income;

	GameManager gameManager;

	protected virtual void Awake()
	{
		gameManager = GameManager.Instance;
	}

	protected virtual void Start()
	{

	}

	protected void EarnGoods()
	{
		gameManager.EarnGoods(goodsType, income);
	}
}
