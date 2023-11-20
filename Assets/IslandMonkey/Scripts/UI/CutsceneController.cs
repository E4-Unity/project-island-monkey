using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
	public GameObject[] cutscenes; // 컷신들을 담을 배열
	public Image whiteFadeImage; // 화면 전체를 덮을 하얀색 Image
	public float fadeDuration = 1f; // 페이드하는 데 걸리는 시간
	public float cutsceneDuration = 1f; // 각 컷신이 표시되는 시간

	void OnEnable()
	{
		whiteFadeImage.color = new Color(1f, 1f, 1f, 0); // 페이드 이미지 초기화
		StartCoroutine(PlayCutscenesSequence());
	}

	IEnumerator PlayCutscenesSequence()
	{
		for (int i = 0; i < cutscenes.Length; i++)
		{
			GameObject cutscene = cutscenes[i];
			cutscene.SetActive(true); // 컷신 활성화

			if (i < cutscenes.Length - 1)
			{
				yield return StartCoroutine(FadeToClear());
				yield return new WaitForSeconds(cutsceneDuration);
				yield return StartCoroutine(FadeToWhite());
			}
			else
			{
				// 마지막 컷신에서는 페이드 인만 실행하고 일정 시간 후에 비활성화
				yield return StartCoroutine(FadeToClear());
				yield return new WaitForSeconds(cutsceneDuration); // 마지막 컷신을 보여주는 시간
			}

			cutscene.SetActive(false); // 컷신 비활성화
		}

		gameObject.SetActive(false); // 모든 컷신이 끝나면 패널 비활성화
	}
	IEnumerator FadeToClear()
	{
		// 페이드 인 동안 화면을 클리어하게 만듭니다 (하얀색에서 투명으로)
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
		// 페이드 아웃 동안 화면을 하얀색으로 만듭니다 (투명에서 하얀색으로)
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
}
