using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsPopup : Popup
{
	[SerializeField] Button yesButton;
	[SerializeField] Button noButton;
	[SerializeField] AdvertisementManager adsmanager;

	public override void Initialize()
	{
		yesButton.onClick.AddListener(() =>
		{
			VoyageUIManager.Hide();
			adsmanager.LoadRewardAd(1);
		});

		noButton.onClick.AddListener(() =>
		{
			VoyageUIManager.Hide();
		});
	}
}
