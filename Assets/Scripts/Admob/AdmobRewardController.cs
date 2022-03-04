using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdmobRewardController : MonoBehaviour
{
    public enum RewardedVideoPlacementId
    {
        RewardedVideo,
        DoubleStars
    }

    public RewardedVideoPlacementId placementId;

    private RewardBasedVideoAd rewardedVideo;

    private string appId;

    private string adUnitIdRewardedVideo;

    private GameObject buyObject;

    private static Base.RewardCallback rewardCallback;

    public static AdmobRewardController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    private void Start()
    {
        adUnitIdRewardedVideo = GameObject.Find("InitObject").GetComponent<AdsInitScript>().adUnitIdRewardedVideo; //ca-app-pub-3940256099942544/5224354917
        rewardedVideo = RewardBasedVideoAd.Instance;
        rewardedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        rewardedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        RequestRewardedVideo(adUnitIdRewardedVideo);
    }

    public void RequestRewardedVideo()
    {
        RequestRewardedVideo(adUnitIdRewardedVideo);
    }

    private void RequestRewardedVideo(string adUnitId)
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedVideo.LoadAd(request, adUnitId);
    }

    public bool IsRewardedVideoLoaded()
    {
        if (rewardedVideo != null)
        {
            return rewardedVideo.IsLoaded();
        }
        return false;
    }

    public void ShowRewardedVideo(Base.RewardCallback callback)
    {
        rewardCallback = callback;
        if (IsRewardedVideoLoaded())
        {
            rewardedVideo.Show();
        }
    }

    private void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        DestroyRewardedVideoNotifications();
        RequestRewardedVideo(adUnitIdRewardedVideo);
    }

    private void HandleRewardBasedVideoRewarded(object sender, EventArgs args)
    {
        rewardCallback();
    }

    private void DestroyRewardedVideoNotifications()
    {

    }
}
