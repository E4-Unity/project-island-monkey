using UnityEngine;
using UnityEngine.UI;
using IslandMonkey;
using DG.Tweening;

public class VoyageRewardPopup : Popup
{
	[SerializeField] Button getButton;
	[SerializeField] SceneLoadingManagerInterface sceneLoadingManagerInterface;
	[SerializeField] Transform boat;

	VoyageDataManager voyageDataManager;
	public override void Initialize()
	{
		voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();
		getButton.onClick.AddListener(() => {
			boat.DOMoveZ(30,30);
			voyageDataManager.ShouldBuild = true;
			sceneLoadingManagerInterface.GoToMainScene();
			this.gameObject.SetActive(false);
			});
	}
}
