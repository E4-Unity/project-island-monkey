using System.Collections;
using IslandMonkey;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하는 경우

public class DrawMachinePanel : MonoBehaviour
{
	[SerializeField] private GameObject drawMachinePanel;
	[SerializeField] private GameObject pickPresentPanel;
	[SerializeField] private Button[] presentButtons; // 선물 상자 버튼들을 연결합니다.
	[SerializeField] private TMP_Text resultText; // 획득한 골드를 표시할 텍스트를 연결합니다. (TextMeshPro 사용 시)

	private GoodsManager goodsManager; // 굿즈 매니저에 대한 참조를 저장합니다.

	void Start()
	{
		// GlobalGameManager에서 GoodsManager를 가져옵니다.
		goodsManager = GlobalGameManager.Instance.GetGoodsManager();
	}

	private void OnEnable()
	{
		// pickPresentPanel을 초기에 비활성화합니다.
		pickPresentPanel.SetActive(false);

		ResetButtons();
		StartCoroutine(ActivatePickPresentPanelAfterDelay(3f));
	}

	private IEnumerator ActivatePickPresentPanelAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		// 지연 후에 drawMachinePanel이 여전히 활성화되어 있을 때만 pickPresentPanel을 활성화합니다.
		if (drawMachinePanel.activeSelf)
		{
			pickPresentPanel.SetActive(true);
		}
	}
	private void ResetButtons()
	{
		foreach (var button in presentButtons)
		{
			if (button != null)
			{
				button.interactable = true;
				button.image.color = Color.white; // 초기 색상을 흰색으로 설정합니다.
			}
		}
	}
	public void SelectPresent(int index)
	{
		// 모든 선물 상자 버튼의 색을 회색으로 변경합니다.
		foreach (var button in presentButtons)
		{
			if (button != null)
			{
				button.interactable = false;
				button.image.color = Color.gray;
			}
		}

		// 골드 획득 로직을 실행합니다.
		StartCoroutine(ShowGoldAcquired());
	}

	private IEnumerator ShowGoldAcquired()
	{
		int[] goldAmounts = new int[] { 1000, 3000, 5000 };
		int gold = goldAmounts[Random.Range(0, goldAmounts.Length)];

		resultText.text = $"골드 {gold} 만큼 받았습니다!!";
		resultText.gameObject.SetActive(true);

		yield return new WaitForSeconds(3f);

		resultText.gameObject.SetActive(false);
		goodsManager.EarnGoods(GoodsType.Gold, gold);
		drawMachinePanel.SetActive(false);
	}
}
