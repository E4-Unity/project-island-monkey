using UnityEngine;
using UnityEngine.UI;
using IslandMonkey;
using DG.Tweening;

public class VoyageTimer : MonoBehaviour
{
	[SerializeField] Image timer;

	VoyageDataManager voyageDataManager;
	private void Start()
	{
		voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();

		timer.fillAmount = voyageDataManager.TimeLeftRatio;
		timer.DOFillAmount(0, voyageDataManager.Timer);

		voyageDataManager.OnBuildingFinished += VoyageFinished;

		if(voyageDataManager.CurrentBuildingData.IsBuildCompleted)
		{
			VoyageFinished();
		}
	}

	public void VoyageFinished()
	{
		VoyageUIManager.Show<VoyageRewardPopup>(false);
	}

}
