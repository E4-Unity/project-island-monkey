using UnityEngine;
using UnityEngine.UI;
using IslandMonkey;
using DG.Tweening;

public class VoyageTimer : MonoBehaviour
{
	[SerializeField] Image timer;

	VoyageDataManager voyageDataManager;

	float currentVelocity = 0;
	float currentValue;
	private void Start()
	{
		voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();
		timer.fillAmount = voyageDataManager.TimeLeftRatio;
		timer.DOFillAmount(0, voyageDataManager.Timer);
		
		
		if (voyageDataManager.Timer == 0)
		{
			VoyageFinished();
		}
		else
		{
			voyageDataManager.OnBuildingFinished += VoyageFinished;
		}
	}

	private void FixedUpdate()
	{
		Debug.Log("Timer : " + voyageDataManager.TimeLeftRatio);
		Debug.Log("Fillamount : " + timer.fillAmount);
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
