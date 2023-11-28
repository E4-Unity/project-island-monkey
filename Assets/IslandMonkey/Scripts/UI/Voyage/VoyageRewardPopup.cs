using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IslandMonkey;

public class VoyageRewardPopup : Popup
{
	[SerializeField] Button getButton;
	[SerializeField] SceneLoadingManagerInterface sceneLoadingManagerInterface;

	VoyageDataManager voyageDataManager;
	public override void Initialize()
	{
		voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();
		getButton.onClick.AddListener(() => {
			voyageDataManager.ShouldBuild = true;
			sceneLoadingManagerInterface.GoToMainScene();
			});
	}
}
