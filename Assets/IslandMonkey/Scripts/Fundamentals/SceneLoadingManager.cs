using System.Collections;
using E4.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IslandMonkey
{
	public enum BuildScene
	{
		None = -1,
		Title = 0,
		Main = 1,
		Voyage = 2
	}
	public class SceneLoadingManager : GenericMonoSingleton<SceneLoadingManager>
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void GenerateSceneLoadingManager()
		{
			var sceneLoadingManager = Resources.Load<SceneLoadingManager>("Scene Loading Manager");
			Instantiate(sceneLoadingManager);
		}

		[SerializeField] Image fadeImage;
		[SerializeField] float fadeDelayTime = 0.3f;
		[SerializeField] float fadeTime = 1f;
		bool isLoading;

		float timer = 0f;

		protected override void Awake()
		{
			base.Awake();
			SceneManager.sceneLoaded += OnSceneLoaded_Event;
		}

		void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		public void ChangeScene(BuildScene buildScene)
		{
			if (isLoading || buildScene == BuildScene.None) return;
			isLoading = true;

			switch (buildScene)
			{
				case BuildScene.Voyage:
					if (!GlobalGameManager.Instance.GetVoyageDataManager().CanEnterVoyageScene)
					{
#if UNITY_EDITOR
						Debug.LogWarning("유학중인 숭숭이가 없습니다.");
#endif
						return;
					}
					break;
				default:
					break;
			}

			LoadScene(buildScene);
		}

		void LoadScene(BuildScene buildScene)
		{
			StartCoroutine(LoadScene((int)buildScene, 0.1f));
		}

		IEnumerator FadeIn() => Fade(0, 1);
		IEnumerator FadeOut() => Fade(1, 0);

		IEnumerator Fade(float start, float end)
		{
			timer = 0;
			SetImageAlpha(fadeImage, start);

			while (timer < fadeTime)
			{
				yield return null;
				timer += Time.deltaTime;
				var alpha = Mathf.Lerp(start, end, timer / fadeTime);
				SetImageAlpha(fadeImage, alpha);
			}

			SetImageAlpha(fadeImage, end);
		}

		// TODO 비동기 로딩
		IEnumerator LoadScene(int index, float delayTime)
		{
			fadeImage.gameObject.SetActive(true);
			yield return StartCoroutine(FadeIn());
			yield return new WaitForSeconds(delayTime);
			SceneManager.LoadScene(index);
		}

		IEnumerator OnSceneLoaded()
		{
			isLoading = true;

			yield return new WaitForSeconds(fadeDelayTime);
			yield return StartCoroutine(FadeOut());
			fadeImage.gameObject.SetActive(false);

			isLoading = false;
		}

		void OnSceneLoaded_Event(Scene scene, LoadSceneMode mode)
		{
			StartCoroutine(OnSceneLoaded());
		}

		void SetImageAlpha(Image image, float alpha)
		{
			image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
		}
	}
}
