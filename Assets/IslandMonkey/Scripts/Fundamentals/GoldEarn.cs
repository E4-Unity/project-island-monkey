using System.Collections;
using IslandMonkey.MVVM;
using UnityEngine;
using UnityEngine.Events;

public class GoldEarn : MonoBehaviour
{
	public int goldIncome = 100;
	public float goldPopupInterval = 2.0f;
	public GameObject goldPopupImage;
	public UnityEvent goldEarnEvent;
	public float monkeyBankTime = 45f; // 클릭을 감지할 시간 설정

	private bool isGoldPopupClicked = false;
	private float timer = 0f;

	private void Start()
	{
		StartCoroutine(EarnGoldRoutine());
		goldPopupImage.SetActive(false);
	}

	private IEnumerator EarnGoldRoutine()
	{
		while (true)
		{
			goldPopupImage.SetActive(true);
			isGoldPopupClicked = false;
			timer = 0f;

			while (timer < monkeyBankTime && !isGoldPopupClicked)
			{
				timer += Time.deltaTime;
				yield return null;
			}

			if (!isGoldPopupClicked)
			{
				// 클릭이 감지되지 않았을 때 처리
				if (!MonkeyBank.Instance.IsFull)
				{
					// 몽키뱅크에 여유가 있을 경우에만 몽키뱅크에 골드를 추가합니다.
					MonkeyBank.Instance.AddToBank(goldIncome);
					goldPopupImage.SetActive(false);
				}
				// 몽키뱅크가 꽉 찼을 경우, 팝업은 유지됩니다.
				else
				{
					Debug.Log("몽키뱅크 꽉 찼음! 더 이상 골드를 추가하지 않습니다.");
					// 인터벌을 기다리지 않고 계속해서 팝업을 유지합니다.
					continue;
				}
			}
			else
			{
				// 클릭이 감지되었을 때 처리
				goldEarnEvent.Invoke();
			}

			yield return new WaitForSeconds(goldPopupInterval);
		}
	}

	private void OnMouseDown()
	{
		if (goldPopupImage.activeInHierarchy)
		{
			isGoldPopupClicked = true;
			GameManager.instance.EarnGoods(GoodsType.Gold, goldIncome); // 골드를 즉시 GameManager에 추가합니다.
			goldPopupImage.SetActive(false);
		}
	}
}
