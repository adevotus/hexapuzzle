// dnSpy decompiler from Assembly-CSharp.dll class: TopPanel
using System;
using com.vector;
using UnityEngine;

public class TopPanel : BasePanel
{
	protected new void Start()
	{
		base.Start();
	}

	protected new void OnEnable()
	{
		base.OnEnable();
		if (!GameUser.Instance.forceTheme)
		{
			this.waiRotTim = 5f;
		}
	}

	protected new void OnDisable()
	{
		base.OnDisable();
	}

	public void UpdateThemeBtn(float y)
	{
		RectTransform rectTransform = this.themeParentAnim.transform as RectTransform;
		Vector2 anchoredPosition = rectTransform.anchoredPosition;
		anchoredPosition.y = y;
		rectTransform.anchoredPosition = anchoredPosition;

        RectTransform rectTransform2 = this.videoParent.transform as RectTransform;
        Vector2 anchoredPosition2 = rectTransform2.anchoredPosition;
        anchoredPosition2.y = y;
        rectTransform2.anchoredPosition = anchoredPosition2;
    }

	public void SwitchThemeBtn(bool isShow)
	{
		if (isShow)
		{
			this.themeParentAnim.SetInteger("AnimState", 0);
		}
		else
		{
			this.themeParentAnim.SetInteger("AnimState", 1);
		}
	}

	public void OnTheme()
	{
		if (this.isEnd)
		{
			return;
		}
		AudioSystem.PlayOneShotEffect("btn");
		GameUser instance = GameUser.Instance;
		instance.nowTheme++;
		instance.nowTheme = ((instance.nowTheme < UIManager.selfInstance.bkgPanel.themeCount) ? instance.nowTheme : 0);
		UIManager.selfInstance.bkgPanel.SelectTheme(instance.nowTheme);
		DateTime now = DateTime.Now;
		if (!instance.forceChristmas && ((now.Month >= 12 && now.Day >= 22) || (now.Month <= 1 && now.Day <= 3)))
		{
			instance.forceChristmas = true;
		}
		instance.forceTheme = true;
		UIManager.selfInstance.menuPanel.SwitchSpTheme();
		VectorAds.invokeAds(109, "theme" + instance.nowTheme.ToString());
		GameUser.Save();
		if (!UIManager.selfInstance.revivePanel.gameObject.activeSelf)
		{
			this.DelayNgs(0.32f);
		}
	}

    public void OnRandomReward()
    {
        AdsInitScript._instance.ShowRewardByType(delegate
        {
            Base._instance.randomRewardindex = UnityEngine.Random.Range(0, 3);
            Base._instance.randomRewardCount = UnityEngine.Random.Range(2, 4);
            switch (Base._instance.randomRewardindex)
            {
                case 0:
                    RewardScriptableObject.instance.tipRemoveCount += Base._instance.randomRewardCount;
                    break;
                case 1:
                    RewardScriptableObject.instance.tipLightCount += Base._instance.randomRewardCount;
                    break;
                case 2:
                    RewardScriptableObject.instance.tipUndoCount += Base._instance.randomRewardCount;
                    break;
            }
            UIManager.selfInstance.rewardPopup.gameObject.SetActive(true);
            Base._instance.UpdateCount();
        });
    }

	internal void DelayNgs(float delayTim)
	{
		if (UIManager.selfInstance.VAinstance.adData.busyAd)
		{
			base.CancelInvoke("InvokeAds");
			base.Invoke("InvokeAds", delayTim);
		}
	}

	private void InvokeAds()
	{
		UIManager.selfInstance.VAinstance.ShowNGS(false);
	}

	private new void Update()
	{
		if (this.waiRotTim > 0f)
		{
			this.waiRotTim -= Time.deltaTime;
			if (this.waiRotTim <= 0f)
			{
				this.waiRotTim = -1f;
				if (!GameUser.Instance.forceTheme)
				{
					this.rotTim = 1f;
				}
			}
		}
		else if (this.rotTim > 0f)
		{
			this.rotTim -= Time.deltaTime * 1f;
			Vector3 localEulerAngles = this.themeIcon.localEulerAngles;
			localEulerAngles.z = Mathf.Lerp(-360f, 0f, this.rotTim);
			if (this.rotTim <= 0f)
			{
				localEulerAngles.z = 0f;
				this.rotTim = -1f;
				this.waiRotTim = UnityEngine.Random.Range(30f, 60f);
			}
			this.themeIcon.localEulerAngles = localEulerAngles;
		}
	}

	public Animator themeParentAnim;

    public GameObject videoParent;

	public RectTransform themeIcon;

	private float waiRotTim = -1f;

	private float rotTim = -1f;
}
