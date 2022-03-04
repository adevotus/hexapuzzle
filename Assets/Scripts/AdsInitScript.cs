using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsInitScript : MonoBehaviour
{
    public string adUnitIdInsterstitial;

    public string adUnitIdRewardedVideo;

    public string adUnitIdBanner;

    public bool useAdmobRewards;

    public string appId;

    public List<AdEvents> adsEvents = new List<AdEvents>();

    public static AdsInitScript _instance;

    private GameState GameStatus;

    public GameState gameStatus
    {
        get
        {
            return GameStatus;
        }
        set
        {
            GameStatus = value;
            AdsInitScript adsInitScript = AdsInitScript._instance;
            adsInitScript.CheckAdsEvents(value);
        }
    }

    private void Awake()
    {
        _instance = this;
        if (_instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    public void CheckAdsEvents(GameState state)
    {
        foreach (AdEvents item in adsEvents)
        {
            if (item.adPlacement == state)
            {
                ShowAdsByType(item.adType);
            }
        }
    }

    public void ShowRewardByType(Base.RewardCallback callback)
    {
        if (useAdmobRewards)
        {
            AdmobRewardController.instance.ShowRewardedVideo(callback);
        } else
        {
            Base._instance.ShowRewarded(callback);
        }
    }

    public void ShowAdsByType(AdType adType)
    {
        switch (adType)
        {
            case AdType.Admob:
                AdmobController.instance.ShowInterstitial();
                break;
            case AdType.Unity:
                Base.ShowInters();
                break;
        }
    }

    public void CheckDaily()
    {
        int num = PlayerPrefs.GetInt("dailyDay", 1);
        DateTime dateTime = DateTime.Now;
        if (!PlayerPrefs.HasKey("SaveDate"))
        {
            PlayerPrefs.SetString("SaveDate", dateTime.ToBinary().ToString());
        }
        else
        {
            DateTime d = DateTime.FromBinary(long.Parse(PlayerPrefs.GetString("SaveDate")));
            num = dateTime.Day - d.Day;
        }
        if (!PlayerPrefs.HasKey("daily" + num.ToString()))
        {
            UnityEngine.Debug.Log("day : " + num);
            PlayerPrefs.SetInt("dailyDay", num);
            PlayerPrefs.SetInt("daily" + num.ToString(), num);
            PlayerPrefs.Save();
        }

    }
}
