using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RollupFooter : MonoBehaviour
{
	public Button rollupBtn;
	public RectTransform footer; // RectTransform으로 변경
	public float targetY = -550f; // Footer가 내려갈 Y 좌표
	public float duration = 0.5f; // 움직임의 지속 시간

	private void Start()
	{
		// 버튼 클릭 리스너 추가
		rollupBtn.onClick.AddListener(ToggleFooterPosition);
	}

	private void ToggleFooterPosition()
	{
		// 현재 위치가 아니라 anchoredPosition을 기준으로 이동합니다.
		float newY = footer.anchoredPosition.y + (footer.anchoredPosition.y == 0 ? targetY : -targetY);
		footer.DOAnchorPosY(newY, duration);
	}
}
