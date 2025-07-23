using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class GoogleAdManager
{
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    //private string _adUnitId = "ca-app-pub-5810536654290957/6624831055";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;

    private Action _onRewardEarnedGlobal;
    private Define.RewardAdType _currentRewardAdType;
    private bool _isRewardEarned;

    public void Init()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Logging.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Logging.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Logging.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                RegisterEventHandlers(_rewardedAd);
            });
    }

    public void ShowRewardedAd(Define.RewardAdType rewardAdType, Action onRewardEarned)
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _currentRewardAdType = rewardAdType;
            _onRewardEarnedGlobal = onRewardEarned;
            _isRewardEarned = false;

            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Logging.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                _isRewardEarned = true;

                // 보상 콜백 실행
                if (rewardAdType != Define.RewardAdType.GiveUp)
                {
                    onRewardEarned?.Invoke();
                    ClearCallbacks();
                }
            });
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Logging.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Logging.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Logging.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Logging.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Logging.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();

            if (_currentRewardAdType == Define.RewardAdType.GiveUp && _isRewardEarned)
            {
                _onRewardEarnedGlobal?.Invoke();
                ClearCallbacks();
            }
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Logging.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }

    private void ClearCallbacks()
    {
        _onRewardEarnedGlobal = null;
        _currentRewardAdType = default;
    }

    public void Clear()
    {

    }
}
