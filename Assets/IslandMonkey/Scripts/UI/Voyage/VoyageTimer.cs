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
		Debug.Log(voyageDataManager.Timer);
		timer.DOFillAmount(0, voyageDataManager.Timer);
		Debug.Log(voyageDataManager.Timer);
		
		if (voyageDataManager.Timer == 0)
		{
			VoyageFinished();
		}
		else
		{
			voyageDataManager.OnBuildingFinished += VoyageFinished;
		}
	}

	public void VoyageFinished()
	{
		VoyageUIManager.Show<VoyageRewardPopup>(false);
	}

	private void OnDestroy()
	{
		voyageDataManager.OnBuildingFinished -= VoyageFinished;
	}
}
