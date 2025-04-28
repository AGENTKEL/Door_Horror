using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YandexMobileAds;
using YandexMobileAds.Base;

public class RewardedController : MonoBehaviour
{
    private RewardedAdLoader rewardedAdLoader;
    private RewardedAd rewardedAd;
    private Action onRewardedComplete;
    public Action onRewardGranted;

    private void Awake()
    {
        SetupLoader();
        RequestRewardedAd();
        DontDestroyOnLoad(gameObject);
    }

    private void SetupLoader()
    {
        rewardedAdLoader = new RewardedAdLoader();
        rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
        rewardedAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
    }

    private void RequestRewardedAd()
    {
        string adUnitId = "demo-rewarded-yandex"; // replace with your real id
        AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
        rewardedAdLoader.LoadAd(adRequestConfiguration);
    }

    private void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show();
        }
    }

    public void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
    {
        rewardedAd = args.RewardedAd;

        rewardedAd.OnAdClicked += HandleAdClicked;
        rewardedAd.OnAdShown += HandleAdShown;
        rewardedAd.OnAdFailedToShow += HandleAdFailedToShow;
        rewardedAd.OnAdImpression += HandleImpression;
        rewardedAd.OnAdDismissed += HandleAdDismissed;
        rewardedAd.OnRewarded += HandleRewarded;
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        // log
    }

    public void HandleAdDismissed(object sender, EventArgs args)
    {
        DestroyRewardedAd();
        RequestRewardedAd();
    }

    public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        DestroyRewardedAd();
        RequestRewardedAd();
    }

    public void HandleAdClicked(object sender, EventArgs args) { }
    public void HandleAdShown(object sender, EventArgs args) { }
    public void HandleImpression(object sender, ImpressionData impressionData) { }

    public void HandleRewarded(object sender, Reward args)
    {
        // Called when the user earned a reward
        onRewardGranted?.Invoke(); // <- call the assigned action
        onRewardGranted = null;    // <- clear it after use
    }

    public void DestroyRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
    }

    // NEW METHOD:
    public void ShowTheAdWithCallback(Action onComplete)
    {
        onRewardedComplete = onComplete;
        ShowRewardedAd();
    }

    public void ShowTheAd(Action onReward)
    {
        if (rewardedAd != null)
        {
            onRewardGranted = onReward;
            rewardedAd.Show();
        }
    }
}
