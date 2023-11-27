using UnityEngine;

namespace IslandMonkey
{
	public class SceneLoadingManagerInterface : MonoBehaviour
	{
		public void GoToMainScene()
		{
			SceneLoadingManager.Instance.ChangeScene(BuildScene.Main);
		}

		public void GoToVoyageScene()
		{
			SceneLoadingManager.Instance.ChangeScene(BuildScene.Voyage);
		}
	}
}
