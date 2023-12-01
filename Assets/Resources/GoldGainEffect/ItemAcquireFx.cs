using UnityEngine;
using DG.Tweening;

public class ItemAcquireFx : MonoBehaviour
{
	// Sequence 참조를 저장하기 위한 필드 추가
	private Sequence sequence;

	public void Explosion(Vector2 from, Vector2 to, float explo_range)
	{
		transform.position = from;

		// 이전에 생성된 시퀀스가 있다면 먼저 제거
		if (sequence != null)
		{
			sequence.Kill();
		}

		// 새 시퀀스 생성
		sequence = DOTween.Sequence();
		sequence.Append(transform.DOMove(from + Random.insideUnitCircle * explo_range, 0.25f).SetEase(Ease.OutCubic));
		sequence.Append(transform.DOMove(to, 0.5f).SetEase(Ease.InCubic));
		sequence.AppendCallback(() => { gameObject.SetActive(false); });
	}

	private void OnDestroy()
	{
		// 이 인스턴스에 대한 모든 DOTween 애니메이션을 중지하고 제거합니다.
		sequence?.Kill();
	}
}
