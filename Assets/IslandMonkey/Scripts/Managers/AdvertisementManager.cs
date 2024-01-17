using UnityEngine;
using GoogleMobileAds.Api;

public class AdvertisementManager : MonoBehaviour
{
	private RewardedAd rewardAd; // 보상형 광고

	private string rewardAdId;

	public enum RewardType {
		CoinBooster,
		BoatSpeed,
		LuckyDraw,
		MonkeyBank
	}; 
	private void Start()
	{
		MobileAds.Initialize((InitializationStatus initStatus) =>
		{
			rewardAdId = "ca-app-pub-3940256099942544/5224354917"; // 테스트용 id
		});
	}

	public void LoadRewardAd(int rewardCode)
	{
		RewardType rewardType = 0;
		switch (rewardCode)
		{
			case 0:
				rewardType = RewardType.CoinBooster;
				break;
			case 1:
				rewardType = RewardType.BoatSpeed;
				break;
			case 2:
				rewardType = RewardType.LuckyDraw;
				break;
			case 3:
				rewardType = RewardType.MonkeyBank;
				break;
			default :
				Debug.Log("Cannot load Ads \n Wrong rewardCode : " + rewardCode);
				return;
		}

		if(rewardAd != null)
		{
			rewardAd.Destroy();
			rewardAd = null;
		}

		Debug.Log("Loading reward ad");

		var adRequest = new AdRequest();

		RewardedAd.Load(rewardAdId, adRequest,
			(RewardedAd ad, LoadAdError error) =>
			{
				if(error != null || ad == null)
				{
					Debug.LogError("Reward ad failed to load ad with error : " + error);
					return;
				}

				Debug.Log("Reward ad loaded with response : " + ad.GetResponseInfo());

				rewardAd = ad;

				RegisterEventHandlers(rewardAd);

				ShowRewardAd(rewardType);
			});
	}

	public void ShowRewardAd(RewardType rewardType)
	{
		const string rewardMsg = "Reward Ad rewarded the user. Type : {0}, amount : {1}";

		if(rewardAd != null && rewardAd.CanShowAd())
		{
			rewardAd.Show((Reward reward) =>
			{
				Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
				SendReward(rewardType);
			});
		}
	}

	private void RegisterEventHandlers(RewardedAd ad)
	{
		// Raised when the ad is estimated to have earned money.
		ad.OnAdPaid += (AdValue adValue) =>
		{
			Debug.Log(string.Format("Rewarded ad paid {0} {1}.",
				adValue.Value,
				adValue.CurrencyCode));
		};
		// Raised when an impression is recorded for an ad.
		ad.OnAdImpressionRecorded += () =>
		{
			Debug.Log("Rewarded ad recorded an impression.");
		};
		// Raised when a click is recorded for an ad.
		ad.OnAdClicked += () =>
		{
			Debug.Log("Rewarded ad was clicked.");
		};
		// Raised when an ad opened full screen content.
		ad.OnAdFullScreenContentOpened += () =>
		{
			Debug.Log("Rewarded ad full screen content opened.");
		};
		// Raised when the ad closed full screen content.
		ad.OnAdFullScreenContentClosed += () =>
		{
			Debug.Log("Rewarded ad full screen content closed.");
		};
		// Raised when the ad failed to open full screen content.
		ad.OnAdFullScreenContentFailed += (AdError error) =>
		{
			Debug.LogError("Rewarded ad failed to open full screen content " +
						   "with error : " + error);
			//LoadRewardAd();
		};
	}

	private void SendReward(RewardType rewardType)
	{
		switch (rewardType)
		{
			case RewardType.BoatSpeed:
				//데이터 저장
				Debug.Log("보트 스피드 2배");
				break;
			case RewardType.CoinBooster:
				//데이터 저장
				break;
			case RewardType.LuckyDraw:
				break;
			case RewardType.MonkeyBank:
				break;
		}
	}
}
