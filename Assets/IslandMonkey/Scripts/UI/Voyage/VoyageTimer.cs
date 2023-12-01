using UnityEngine;
using UnityEngine.UI;
using IslandMonkey;
using DG.Tweening;

public class VoyageTimer : MonoBehaviour
{
	[SerializeField] Image timer;

	VoyageDataManager voyageDataManager;

	void OnEnable()
	{
		if (Application.isEditor) //fix dotween's live recompile speed issue
		{
			float someValue = 0f;
			float startTime = Time.time;
			DOTween.To(() => someValue, x => someValue = x, 1f, 1f).OnComplete(() =>
			{
				float timeScale = Time.time - startTime;
				Debug.Log($"Dotween TimeScale: {Time.time - startTime}");
				if (timeScale < 0.9f)
					DOTween.timeScale = timeScale;
			});
		}
	}

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
