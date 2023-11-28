using System.Collections;
using E4.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandMonkey
{
	public enum BuildScene
	{
		None = -1,
		Main = 0,
		Voyage = 1
	}
	public class SceneLoadingManager : GenericMonoSingleton<SceneLoadingManager>
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void GenerateSceneLoadingManager()
		{
			var sceneLoadingManager = Resources.Load<SceneLoadingManager>("Scene Loading Manager");
			Instantiate(sceneLoadingManager);
		}

		[SerializeField] CanvasGroup canvasGroup;
		[SerializeField] float fadeDelayTime = 0.3f;
		[SerializeField] float fadeTime = 1f;
		bool isLoading;

		bool IsLoading
		{
			get => isLoading;
			set
			{
				isLoading = value;
				canvasGroup.gameObject.SetActive(value);
			}
		}

		float timer = 0f;

		protected override void Awake()
		{
			base.Awake();
			SceneManager.sceneLoaded += (arg0, mode) => FadeOut();
		}

		void Start()
		{
			DontDestroyOnLoad(gameObject);

			IsLoading = true;
		}

		public void ChangeScene(BuildScene buildScene)
		{
			if (IsLoading || buildScene == BuildScene.None) return;

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

			IsLoading = true;
			LoadScene(buildScene);
		}

		void LoadScene(BuildScene buildScene)
		{
			FadeIn();
			StartCoroutine(LoadScene((int)buildScene, fadeDelayTime + fadeTime + 0.1f));
		}

		void FadeIn()
		{
			StartCoroutine(Fade(0, 1, false));
		}

		void FadeOut()
		{
			StartCoroutine(Fade(1, 0));
		}

		IEnumerator Fade(float start, float end, bool endLoading = true)
		{
			yield return new WaitForSeconds(fadeDelayTime);

			while (timer < fadeTime)
			{
				yield return null;
				timer += Time.deltaTime;
				var alpha = Mathf.Lerp(start, end, timer / fadeTime);
				canvasGroup.alpha = alpha;
			}

			timer = 0;
			canvasGroup.alpha = end;
			IsLoading = !endLoading;
		}

		// TODO 비동기 로딩
		IEnumerator LoadScene(int index, float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			SceneManager.LoadScene(index);
		}
	}
}
