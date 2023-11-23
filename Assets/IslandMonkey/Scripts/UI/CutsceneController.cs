using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
	[SerializeField] private GameObject[] cutscenes; // 컷신들을 담을 배열
	[SerializeField] private GameObject cutscenesBG; // 컷신들을 담을 배경
	[SerializeField] private Image whiteFadeImage; // 화면 전체를 덮을 하얀색 Image
	[SerializeField] private float fadeDuration = 1f; // 페이드하는 데 걸리는 시간
	[SerializeField] private float cutsceneDuration = 1f; // 각 컷신이 표시되는 시간
	[SerializeField] private Transform smallCircle; // 원 UI의 Transform 컴포넌트
	[SerializeField] private Camera cutsceneCamera; // 컷신용 카메라에 대한 참조
	public event Action OnCutSceneEnd;

	void OnEnable()
	{
		whiteFadeImage.color = new Color(1f, 1f, 1f, 0); // 페이드 이미지 초기화
		StartCoroutine(PlayCutscenesSequence());
	}

	IEnumerator PlayCutscenesSequence()
	{
		cutscenesBG.SetActive(true);
		// 기존 컷신 재생
		foreach (GameObject cutscene in cutscenes)
		{
			cutscene.SetActive(true); // 컷신 활성화
			yield return StartCoroutine(FadeToClear()); // 페이드 인
			yield return new WaitForSeconds(cutsceneDuration); // 지속 시간 동안 대기
			yield return StartCoroutine(FadeToWhite()); // 페이드 아웃
			cutscene.SetActive(false); // 컷신 비활성화
		}

		cutscenesBG.SetActive(false);
		// 카메라 위치 변경 및 추가 연출 실행
		cutsceneCamera.transform.position = new Vector3(0, 6, -11);
		cutsceneCamera.orthographicSize = 1; // Orthographic Size를 1로 설정
		smallCircle.gameObject.SetActive(true); // 원 UI 활성화
		yield return StartCoroutine(ScaleCircle(smallCircle, Vector3.zero, Vector3.one, 0.5f)); // 커지는 연출

		// 캐릭터 등장 연출
		OnCutSceneEnd?.Invoke();

		yield return new WaitForSeconds(5f); // 5초 동안 유지
		yield return StartCoroutine(ScaleCircle(smallCircle, Vector3.one, Vector3.zero, 0.5f)); // 작아지는 연출
		smallCircle.gameObject.SetActive(false); // 원 UI 비활성화

		gameObject.SetActive(false); // 컷신 컨트롤러 비활성화
	}

	IEnumerator FadeToClear()
	{
		float time = 0f;
		while (time < fadeDuration)
		{
			float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
			whiteFadeImage.color = new Color(1f, 1f, 1f, alpha);
			time += Time.deltaTime;
			yield return null;
		}
		whiteFadeImage.color = Color.clear;
	}

	IEnumerator FadeToWhite()
	{
		float time = 0f;
		while (time < fadeDuration)
		{
			float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
			whiteFadeImage.color = new Color(1f, 1f, 1f, alpha);
			time += Time.deltaTime;
			yield return null;
		}
		whiteFadeImage.color = Color.white;
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
