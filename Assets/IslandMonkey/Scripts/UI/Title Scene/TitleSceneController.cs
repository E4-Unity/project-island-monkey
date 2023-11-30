using System.Collections;
using UnityEngine;

namespace IslandMonkey
{
	public class TitleSceneController : MonoBehaviour
	{
		IEnumerator LoadMainScene()
		{
			yield return new WaitForSeconds(2);

			SceneLoadingManager.Instance.ChangeScene(BuildScene.Main);
		}

		void Start()
		{
			StartCoroutine(LoadMainScene());
		}
	}
}
