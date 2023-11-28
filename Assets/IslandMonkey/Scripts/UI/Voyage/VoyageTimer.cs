using UnityEngine;
using UnityEngine.UI;
using IslandMonkey;

public class VoyageTimer : MonoBehaviour
{
	[SerializeField] Image timer;

	VoyageDataManager voyageDataManager;
	private void Start()
	{
		voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();
		voyageDataManager.OnBuildingFinished += VoyageFinished;

		if(voyageDataManager.CurrentBuildingData.IsBuildCompleted)
		{
			VoyageFinished();
		}
	}
	private void Update()
	{
		timer.fillAmount = voyageDataManager.TimeLeftRatio;
	}

	public void VoyageFinished()
	{
		VoyageUIManager.Show<VoyageRewardPopup>(false);
	}

}
