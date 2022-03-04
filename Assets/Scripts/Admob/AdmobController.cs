using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdmobController : MonoBehaviour
{
    private BannerView bannerView;

    private InterstitialAd interstitialAd;

    private float timeForNextUpdate;

    private string appId;


    private string adUnitIdBanner;

    private string adUnitIdInsterstitial;

    private bool isBannerSummaryVisible = true;

    public static AdmobController instance;

    private void Awake()
    {
        instance = this;
        if (instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("AdmobInit") != 1)
        {
            appId = GameObject.Find("InitObject").GetComponent<AdsInitScript>().appId; //ca-app-pub-3940256099942544~3347511713
            MobileAds.SetiOSAppPauseOnBackground(pause: true);
            MobileAds.Initialize(status => { PlayerPrefs.SetInt("AdmobInit", 1); });
        }
        adUnitIdBanner = GameObject.Find("InitObject").GetComponent<AdsInitScript>().adUnitIdBanner; //ca-app-pub-3940256099942544/6300978111
        adUnitIdInsterstitial = GameObject.Find("InitObject").GetComponent<AdsInitScript>().adUnitIdInsterstitial; //ca-app-pub-3940256099942544/1033173712
        RequestBanner(adUnitIdBanner, out bannerView);
        RequestInterstitial(adUnitIdInsterstitial);
        HideBannerSummary();
    }

    private void Update()
    {
        //timeForNextUpdate += Time.deltaTime;
        //if (timeForNextUpdate >= updateInterval)
        //{
        //    timeForNextUpdate -= updateInterval;
        //    if (!Globals.showAds)
        //    {
        //        bannerViewPause.Destroy();
        //        bannerViewSummary.Destroy();
        //        interstitialAd.Destroy();
        //        UnityEngine.Object.Destroy(base.gameObject);
        //    }
        //}
    }

    public void ShowBannerSummary()
    {
        if (!isBannerSummaryVisible && !PlayerPrefs.HasKey("removeAds"))
        {
            isBannerSummaryVisible = true;
            bannerView.Show();
        }
    }

    public void HideBannerSummary()
    {
        if (isBannerSummaryVisible)
        {
            isBannerSummaryVisible = false;
            bannerView.Hide();
        }
    }

    private void RequestBanner(string adUnitId, out BannerView bannerView)
    {
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }

    private void RequestInterstitial(string adUnitId)
    {
        interstitialAd = new InterstitialAd(adUnitId);
        interstitialAd.OnAdClosed += OnAdClosed;
        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);
    }

    private void OnAdClosed(object sender, EventArgs args)
    {
        DestroyInterstitial();
        RequestInterstitial(adUnitIdInsterstitial);
    }

    public bool IsInterstitialLoaded()
    {
        return interstitialAd.IsLoaded();
    }

    public void ShowInterstitial()
    {
        if (IsInterstitialLoaded() && !PlayerPrefs.HasKey("removeAds"))
        {
            interstitialAd.Show();
        }
        RequestInterstitial(adUnitIdInsterstitial);
    }

    public void DestroyInterstitial()
    {
        interstitialAd.Destroy();
    }

}
