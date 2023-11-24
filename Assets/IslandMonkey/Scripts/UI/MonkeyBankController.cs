using UnityEngine;
using TMPro; 
using System.Collections;

public class MonkeyBankController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI goldText;
	[SerializeField]
	private TextMeshProUGUI goldPopupText;
	[SerializeField]
	private int currentGold; // 현재 알고 있는 골드의 양
	private MonkeyBank monkeyBank; // MonkeyBank 인스턴스 참조
	

	private void Start()
	{
		monkeyBank = MonkeyBank.Instance; // Singleton 인스턴스 할당
		StartCoroutine(CheckGoldChange()); // 골드 변화 체크 코루틴 시작
		currentGold = monkeyBank.Gold; // 초기 골드 값을 설정
		goldText.text = currentGold.ToString(); // 초기 UI 업데이트
		goldPopupText.text = currentGold.ToString();
	}

	private IEnumerator CheckGoldChange()
	{
		while (true) // 무한 루프
		{
			yield return new WaitForSeconds(0.5f); // 0.5초 마다 실행
			int newGold = monkeyBank.Gold; // 현재 골드 값을 가져옴
			if (newGold != currentGold) // 골드에 변화가 있는지 체크
			{
				currentGold = newGold; // 변화가 있으면 현재 골드 업데이트
				goldText.text = currentGold.ToString(); // UI 업데이트 (숫자만)
			}
		}
	}
}
