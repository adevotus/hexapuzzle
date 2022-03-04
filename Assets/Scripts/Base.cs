using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;
using UnityEngine.UI;

public class Base : MonoBehaviour
{

    [Header("Variable")]
    private static DateTime lastAdsTime;

    public static Base _instance;

    //[HideInInspector]
    //public int countToAds;

    //public Button[] btnAds;

    public delegate void RewardCallback();

    private RewardCallback onRewardedReward;

    private RewardCallback onRewardSkip;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        //PlayerPrefs.DeleteKey("removeAds");
    }

    private void Start()
    {
        string gameId = "3225541";
        if (Advertisement.isSupported && !Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameId);
        }
        Debug.Log("Ads: Unity rewarded Ads intialized " + Advertisement.isInitialized);

        //countToAds = 1;
        lastAdsTime = DateTime.Now;

        /// default is 60 sec
        //StartCoroutine(CheckVideoButtonRoutine(60));
        StartCoroutine(CheckAvailableRoutine());
    }

    private static AndroidJavaObject getCurrentActivity()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        return jc.GetStatic<AndroidJavaObject>("currentActivity");
    }


    #region RewardAds
    public bool IsRewardReady()
    {
        return Advertisement.IsReady();
    }

    /// Auto check available video
    //IEnumerator CheckVideoButtonRoutine(float timeCount)
    //{
    //    if (btnAds[0] != null)
    //        btnAds[0].interactable = IsRewardReady();
    //    yield return new WaitForSeconds(timeCount);
    //}

    // Show Reward without skipable
    public void ShowRewarded(RewardCallback onRewardComplete)
    {
        if (IsRewardReady())
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = AdCallbackhanler;
            onRewardedReward = onRewardComplete;
            if (IsRewardReady())
            {
                Advertisement.Show(options);
            }
        }
        else
        {
            noAdsPopup.gameObject.SetActive(true);
        }
    }

    // Show Reward with skipable
    public void ShowRewarded(RewardCallback onRewardComplete, RewardCallback onRewardFail)
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = AdCallbackhanler;
        onRewardedReward = onRewardComplete;
        onRewardSkip = onRewardFail;
        if (IsRewardReady())
        {
            Advertisement.Show(options);
        }
    }

    void AdCallbackhanler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                onRewardedReward();
                break;
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                onRewardSkip();
                break;
        }
    }
    #endregion


    #region LogEvent
    // Log  Event with parameter
    public static void LogEvent(string customEvent)
    {
#if UNITY_EDITOR
        Debug.Log("LogEvent_" + customEvent);
#elif UNITY_ANDROID
        var activity = getCurrentActivity();
        if (activity != null)
        {
            activity.Call("l", new object[] {
                customEvent
            });
        }
#endif
    }
    #endregion


    #region Inters
    // Config time tren Unity Dashboard, default = 40s
    private static bool IsInterReady()
    {
        return (DateTime.Now - lastAdsTime).TotalSeconds > RemoteSettings.GetInt("AdCountTime", 60) && !PlayerPrefs.HasKey("removeAds");
    }

    public static void ShowInters()
    {
        if (IsInterReady())
        {
            Advertisement.Show();
        }
    }
    #endregion

    #region Modify_2248_Ads
    public GameObject removeVideo;
    public GameObject lightVideo;
    public GameObject undoViedo;
    public Text removeCount;
    public Text ligtCount;
    public Text undoCount;
    public Transform noAdsPopup;
    public Sprite videoEnable;
    public Sprite videoDisable;
    public int randomRewardindex;
    public int randomRewardCount;

    IEnumerator CheckAvailableRoutine()
    {
        if (IsRewardReady())
        {
            removeVideo.GetComponent<Image>().sprite = videoEnable;
            lightVideo.GetComponent<Image>().sprite = videoEnable;
            undoViedo.GetComponent<Image>().sprite = videoEnable;
        }
        else
        {
            removeVideo.GetComponent<Image>().sprite = videoDisable;
            lightVideo.GetComponent<Image>().sprite = videoDisable;
            undoViedo.GetComponent<Image>().sprite = videoDisable;
        }
        yield return new WaitForSeconds(10);
        StartCoroutine(CheckAvailableRoutine());
    }

    public void UpdateCount()
    {
        removeVideo.SetActive(false);
        lightVideo.SetActive(false);
        undoViedo.SetActive(false);
        removeCount.gameObject.SetActive(true);
        ligtCount.gameObject.SetActive(true);
        undoCount.gameObject.SetActive(true);
        removeCount.text = RewardScriptableObject.instance.tipRemoveCount.ToString();
        ligtCount.text = RewardScriptableObject.instance.tipLightCount.ToString();
        undoCount.text = RewardScriptableObject.instance.tipUndoCount.ToString();
        removeCount.color = Color.black;
        ligtCount.color = Color.black;
        undoCount.color = Color.black;
    }
    #endregion
}