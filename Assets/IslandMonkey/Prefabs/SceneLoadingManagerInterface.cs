using UnityEngine;

namespace IslandMonkey
{
	public class SceneLoadingManagerInterface : MonoBehaviour
	{
		public void GoToMainScene()
		{
			SceneLoadingManager.Instance.ChangeScene(BuildScene.Main, SceneLoadingManager.ChangeSceneType.Animation);
		}

		public void GoToVoyageScene()
		{
			SceneLoadingManager.Instance.ChangeScene(BuildScene.Voyage, SceneLoadingManager.ChangeSceneType.Animation);
		}
	}
}
