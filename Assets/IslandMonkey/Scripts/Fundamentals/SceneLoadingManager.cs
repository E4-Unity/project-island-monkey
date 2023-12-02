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
		public enum ChangeSceneType
		{
			Fade,
			Animation
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void GenerateSceneLoadingManager()
		{
			var sceneLoadingManager = Resources.Load<SceneLoadingManager>("Scene Loading Manager");
			Instantiate(sceneLoadingManager);
		}

		[SerializeField] Image fadeImage;
		[SerializeField] Image animationImage;

		Animator animator;

		[SerializeField] float fadeDelayTime = 0.3f;
		[SerializeField] float fadeTime = 1f;
		bool isLoading;

		float timer = 0f;
		static readonly int HashFadeIn = Animator.StringToHash("FadeIn");
		static readonly int HashFadeOut = Animator.StringToHash("FadeOut");
		static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

		protected override void Awake()
		{
			base.Awake();
			animator = animationImage.GetComponent<Animator>();
			SceneManager.sceneLoaded += OnSceneLoaded_Event;
		}

		void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		public void ChangeScene(BuildScene buildScene, ChangeSceneType changeSceneType = ChangeSceneType.Fade)
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

			LoadScene(buildScene, changeSceneType);
		}

		void LoadScene(BuildScene buildScene, ChangeSceneType changeSceneType)
		{
			StartCoroutine(LoadScene((int)buildScene, 0.1f, changeSceneType));
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
		IEnumerator LoadScene(int index, float delayTime, ChangeSceneType changeSceneType)
		{
			yield return new WaitForSeconds(0.5f);

			if (changeSceneType == ChangeSceneType.Animation)
			{
				animationImage.gameObject.SetActive(true);
				animator.SetTrigger(HashFadeIn);
				yield return StartCoroutine(WaitForAnimationStart());
				yield return StartCoroutine(WaitForAnimationEnd());
			}
			else
			{
				fadeImage.gameObject.SetActive(true);
				yield return StartCoroutine(FadeIn());
			}

			yield return new WaitForSeconds(delayTime);
			SceneManager.LoadScene(index);
		}

		IEnumerator OnSceneLoaded()
		{
			isLoading = true;

			if (animationImage.gameObject.activeSelf)
			{
				// Play Fade Out Animation
				animator.SetTrigger(HashFadeOut);
				yield return StartCoroutine(WaitForAnimationStart());
				yield return StartCoroutine(WaitForAnimationEnd());
				animationImage.gameObject.SetActive(false);
			}
			else
			{
				if(!fadeImage.gameObject.activeSelf) fadeImage.gameObject.SetActive(true);

				// Fade Out Image
				yield return new WaitForSeconds(fadeDelayTime);
				yield return StartCoroutine(FadeOut());

				fadeImage.gameObject.SetActive(false);
			}

			isLoading = false;
		}

		IEnumerator WaitForAnimationStart()
		{
			while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
			{
				yield return waitForFixedUpdate;
			}
		}

		IEnumerator WaitForAnimationEnd()
		{
			while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
			{
				yield return waitForFixedUpdate;
			}
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
