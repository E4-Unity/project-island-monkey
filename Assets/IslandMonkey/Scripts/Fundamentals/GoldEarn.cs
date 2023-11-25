using System.Collections;
using IslandMonkey;
using IslandMonkey.MVVM;
using UnityEngine;
using UnityEngine.Events;

public class GoldEarn : GoodsFactory
{
	public float goldPopupInterval = 2.0f;
	public GameObject goldPopupImage;
	public UnityEvent goldEarnEvent;
	public float monkeyBankTime = 5f; // 클릭을 감지할 시간 설정

	private bool isGoldPopupClicked = false;
	private float timer = 0f;

	MonkeyBank monkeyBank;

	protected override void Start()
	{
		base.Start();

		monkeyBank = GlobalGameManager.Instance.GetMonkeyBank();
		if (monkeyBank is null) return;

		StartCoroutine(EarnGoldRoutine());
		goldPopupImage.SetActive(false);
	}

	private IEnumerator EarnGoldRoutine()
	{
		while (true)
		{
			isGoldPopupClicked = false;
			timer = 0f;

			yield return new WaitForSeconds(2.0f); // 2초 대기 후 골드 팝업을 비활성화
			goldPopupImage.SetActive(false);

			// 2초 대기 후 골드 팝업을 활성화
			yield return new WaitForSeconds(2.0f);
			goldPopupImage.SetActive(true);

			while (timer < monkeyBankTime && !isGoldPopupClicked)
			{
				timer += Time.deltaTime;
				yield return null;
			}

			if (!isGoldPopupClicked)
			{
				if (!monkeyBank.IsFull)
				{
					monkeyBank.AddToBank(Income);
					Debug.Log("몽키뱅크에 돈이 들어가고 있습니다");
				}
				else
				{
					Debug.Log("몽키뱅크 꽉 찼음! 더 이상 골드를 추가하지 않습니다.");
				}
			}
			else
			{
				goldEarnEvent.Invoke();
			}

			// goldPopupInterval 초 대기 후 다시 팝업을 활성화
			yield return new WaitForSeconds(goldPopupInterval);
			goldPopupImage.SetActive(true);
		}
	}

	private void OnMouseDown()
	{
		if (goldPopupImage.activeInHierarchy)
		{
			isGoldPopupClicked = true;
			EarnGoods(); // 골드를 즉시 GameManager에 추가합니다.
			goldPopupImage.SetActive(false);
		}
	}
}
