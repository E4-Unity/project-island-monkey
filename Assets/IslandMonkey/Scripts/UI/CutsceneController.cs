using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
	[SerializeField] private GameObject[] cutscenes; // 컷신들을 담을 배열
	[SerializeField] private GameObject cutscenesBG; // 컷신들을 담을 배경
	[FormerlySerializedAs("whiteFadeImage")] [SerializeField] private Image cardFadeImage; // 카드 화면을 덮을 하얀색 Image
	[SerializeField] private float fadeDuration = 1f; // 페이드하는 데 걸리는 시간
	[SerializeField] private float cutsceneDuration = 1f; // 각 컷신이 표시되는 시간
	[SerializeField] private Transform smallCircle; // 원 UI의 Transform 컴포넌트
	[SerializeField] private Camera cutsceneCamera; // 컷신용 카메라에 대한 참조

	[SerializeField] Image panelFadeImage; // 전체 패널을 덮을 하얀색 Image
	public event Action OnCutSceneEnd;

	void OnEnable()
	{
		cardFadeImage.color = new Color(1f, 1f, 1f, 0); // 페이드 이미지 초기화
		StartCoroutine(PlayCutscenesSequence());
	}

	IEnumerator PlayCutscenesSequence()
	{
		// 카메라 위치 변경
		cutsceneCamera.transform.position = new Vector3(0, 6, -11);
		cutsceneCamera.orthographicSize = 1; // Orthographic Size를 1로 설정

		// 컷신 재생
		cutscenesBG.SetActive(true);
		int cutsceneIndex = 0;

		foreach (GameObject cutscene in cutscenes)
		{
			cutscene.SetActive(true); // 컷신 활성화
			// 컷신 인덱스에 따라 다른 사운드 재생
			switch (cutsceneIndex)
			{
				case 0:
					SoundManager.instance.PlaySoundEffect("Cutscene_1");
					break;
				case 1:
					SoundManager.instance.PlaySoundEffect("Cutscene_2");
					break;
				case 2:
					SoundManager.instance.PlaySoundEffect("Cutscene_3");
					Handheld.Vibrate();
					break;
			}

			yield return StartCoroutine(FadeToClear(cardFadeImage, fadeDuration)); // 페이드 인
			yield return new WaitForSeconds(cutsceneDuration); // 지속 시간 동안 대기
			if (cutsceneIndex != 2)
			{
				yield return StartCoroutine(FadeToWhite(cardFadeImage, fadeDuration)); // 페이드 아웃
				cutscene.SetActive(false); // 컷신 비활성화
				cutsceneIndex++; // 다음 컷신을 위한 인덱스 증가
			}
		}

		// 패널 페이드 인
		yield return StartCoroutine(FadeToWhite(panelFadeImage, 1.25f));
		cutscenesBG.SetActive(false);

		// 추가 연출 실행
		smallCircle.gameObject.SetActive(true); // 원 UI 활성화

		StartCoroutine(FadeToClear(panelFadeImage, 0.5f)); // 패널 페이드 아웃
		yield return StartCoroutine(ScaleCircle(smallCircle, Vector3.zero, Vector3.one, 1f)); // 커지는 연출

		// 캐릭터 등장 연출
		OnCutSceneEnd?.Invoke();
		SoundManager.instance.PlaySoundEffect("Acquisition_Monkey");
	}

	IEnumerator FadeToClear(Image target, float fadeTime)
	{
		float time = 0f;
		while (time < fadeTime)
		{
			float alpha = Mathf.Lerp(1f, 0f, time / fadeTime);
			target.color = new Color(1f, 1f, 1f, alpha);
			time += Time.deltaTime;
			yield return null;
		}
		target.color = Color.clear;
	}

	IEnumerator FadeToWhite(Image target, float fadeTime)
	{
		float time = 0f;
		while (time < fadeTime)
		{
			float alpha = Mathf.Lerp(0f, 1f, time / fadeTime);
			target.color = new Color(1f, 1f, 1f, alpha);
			time += Time.deltaTime;
			yield return null;
		}
		target.color = Color.white;
	}

	IEnumerator ScaleCircle(Transform targetTransform, Vector3 startScale, Vector3 endScale, float duration)
	{
		float time = 0f;
		Image maskChildImage = targetTransform.GetComponentInChildren<Image>(); // 마스크 자식 Image 찾기
		while (time < duration)
		{
			float scale = Mathf.Lerp(startScale.x, endScale.x, time / duration);
			maskChildImage.rectTransform.localScale = new Vector3(scale, scale, 1); // Image 크기 변경
			time += Time.deltaTime;
			yield return null;
		}
	}
}
