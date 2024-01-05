using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RollupFooter : MonoBehaviour
{
	public Button rollupBtn;
	public RectTransform footer; // RectTransform으로 변경
	public float targetY = -800f; // Footer가 내려갈 Y 좌표
	public RectTransform rollupButtonRectTransform; // 추가: 버튼의 RectTransform
	public float buttonTargetY = -600f; // 추가: 버튼이 내려갈 Y 좌표
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

		// 버튼 이동 - 추가
		float newButtonY = rollupButtonRectTransform.anchoredPosition.y -
		                   (rollupButtonRectTransform.anchoredPosition.y == 0 ? buttonTargetY : -buttonTargetY);
		rollupButtonRectTransform.DOAnchorPosY(newButtonY, duration);
	}
	private void OnDestroy()
	{
		// 이 오브젝트에 대한 모든 DOTween 애니메이션을 즉시 완료된 상태로 중지하고 제거합니다.
		footer.DOKill(true);
		rollupButtonRectTransform.DOKill(true);
	}
}
