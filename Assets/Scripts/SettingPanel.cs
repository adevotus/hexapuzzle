// dnSpy decompiler from Assembly-CSharp.dll class: SettingPanel
using System;
using com.vector;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
	protected new void Start()
	{
		base.Start();
		if (AudioSystem.isSound)
		{
			this.soundOn.SetActive(true);
			this.soundOff.SetActive(false);
		}
		else
		{
			this.soundOn.SetActive(false);
			this.soundOff.SetActive(true);
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			this.rateGo.SetActive(false);
			if (Storage.ReadConfig("refusenotification", "false") == "false")
			{
				this.notfiOnGo.SetActive(true);
				this.notfiOffGo.SetActive(false);
			}
			else
			{
				this.notfiOnGo.SetActive(false);
				this.notfiOffGo.SetActive(true);
			}
		}
		else
		{
			this.rateGo.SetActive(true);
			this.notfiOnGo.SetActive(false);
			this.notfiOffGo.SetActive(false);
		}
	}

	protected new void OnEnable()
	{
		base.OnEnable();
		if (Localization.getCurFlag() == "China")
		{
			this.imgNoAds.sprite = this.cnNoAds;
		}
		else
		{
			this.imgNoAds.sprite = this.enNoAds;
		}
		UIManager.selfInstance.topPanel.SwitchThemeBtn(true);
		KeyBoardListener kbl = UIManager.selfInstance.kbl;
		kbl.onBackKeyEvent = (KeyBoardListener.OnBackKeyEvent)Delegate.Combine(kbl.onBackKeyEvent, new KeyBoardListener.OnBackKeyEvent(this.OnBackKeyEvent));
	}

	protected new void OnDisable()
	{
		base.OnDisable();
		KeyBoardListener kbl = UIManager.selfInstance.kbl;
		kbl.onBackKeyEvent = (KeyBoardListener.OnBackKeyEvent)Delegate.Remove(kbl.onBackKeyEvent, new KeyBoardListener.OnBackKeyEvent(this.OnBackKeyEvent));
	}

	private void OnBackKeyEvent()
	{
		this.OnClose();
	}

	public void OnAnimInAfter()
	{
		if (UIManager.selfInstance.VAinstance.adData.busyAd)
		{
			UIManager.selfInstance.VAinstance.ShowNGS(false);
		}
	}

	public void OnClose()
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		base.NextPanel(UIManager.selfInstance.prePanel.gameObject);
	}

	public void OnSoundSwitch(bool isOpen)
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.SwitchSound(isOpen);
		if (AudioSystem.isSound)
		{
			this.soundOn.SetActive(true);
			this.soundOff.SetActive(false);
		}
		else
		{
			this.soundOn.SetActive(false);
			this.soundOff.SetActive(true);
		}
		AudioSystem.PlayOneShotEffect("btn");
	}

	public void OnNotificationSwitch(bool isOpen)
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		if (isOpen)
		{
			this.notfiOnGo.SetActive(true);
			this.notfiOffGo.SetActive(false);
			UIManager.selfInstance.menuPanel.RequestLocalNotice();
		}
		else
		{
			this.notfiOnGo.SetActive(false);
			this.notfiOffGo.SetActive(true);
			VectorNative.invokeNative(124, "2248hexa_daily");
			Storage.WriteConfig("refusenotification", "true");
		}
	}

	public void OnRate()
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		VectorNative.invokeNative(106, string.Empty);
	}

	public void OnFbLike()
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		VectorNative.invokeNative(109, string.Empty);
	}

	public void OnTwitter()
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		VectorNative.invokeNative(110, string.Empty);
	}

	public void OnRank()
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		if (!InitScript.isLoginGameCenter)
		{
			VectorNative.invokeNative(128, string.Empty);
			InitScript.isLoginGameCenter = true;
		}
		if (InitScript.isLoginGameCenter)
		{
			string text = "CgkI_PC18bcVEAIQAA";
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				text = "nlhexa_leaderboard";
			}
			long num = GameUser.Instance.bestRecord;
			if (UIManager.selfInstance.gamePanel.targetScore > num)
			{
				num = UIManager.selfInstance.gamePanel.targetScore;
			}
			VectorNative.invokeNative(102, string.Concat(new object[]
			{
				"{\"category\":\"",
				text,
				"\",\"score\":",
				num,
				"}"
			}));
			VectorNative.invokeNative(105, string.Concat(new string[]
			{
				"{    \"category\": \"",
				text,
				"\",\"timeScope\": ",
				UIManager.selfInstance.VAinstance.adData.rankScope.ToString(),
				"}"
			}));
		}
	}

	public void OnMoreGame()
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		VectorNative.invokeNative(104, string.Empty);
	}

	public void OnRemoveAds()
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		UIManager.selfInstance.noAdsThisTime = true;
		VectorAds.invokeAds(109, "setting_noads");
		VectorNative.invokeNative(112, "nlhexa_noads");
	}

	public void OnRestore()
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		VectorNative.invokeNative(113, string.Empty);
	}

	private new void Update()
	{
	}

	public GameObject soundOn;

	public GameObject soundOff;

	public Sprite enNoAds;

	public Sprite cnNoAds;

	public Image imgNoAds;

	public GameObject notfiOnGo;

	public GameObject notfiOffGo;

	public GameObject rateGo;

	internal const string ANDROID_LEADERBOARD = "CgkI_PC18bcVEAIQAA";

	internal const string IOS_LEADERBOARD = "nlhexa_leaderboard";

	internal const string IAP_NOADS = "nlhexa_noads";
}
