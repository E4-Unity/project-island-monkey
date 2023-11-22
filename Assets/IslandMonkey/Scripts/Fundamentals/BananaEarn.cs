using System.Collections;
using System.Collections.Generic;
using IslandMonkey.MVVM;
using UnityEngine;
using UnityEngine.Events;

public class BananaEarn : MonoBehaviour
{
	public int bananaIncome = 100;
	public float bananaEarnInterval = 1.0f;
	public UnityEvent bananaEarnEvent;

	private void Start()
	{
		StartCoroutine(EarnBananaRoutine());
	}

	private IEnumerator EarnBananaRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(bananaEarnInterval);
			GameManager.instance.EarnGoods(GoodsType.Banana, bananaIncome);
			bananaEarnEvent.Invoke();
		}
	}
}
