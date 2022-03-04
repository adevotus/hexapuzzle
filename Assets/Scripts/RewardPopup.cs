using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    public Text bodyText;

    public Image tipImg;

    public Sprite removeSpr;

    public Sprite lightSpr;

    public Sprite undoSpr;

    private void OnEnable()
    {
        switch (Base._instance.randomRewardindex)
        {
            case 0:
                tipImg.sprite = removeSpr;
                bodyText.text = "You've Got " + Base._instance.randomRewardCount + " Remove Block Tips";
                break;
            case 1:
                tipImg.sprite = lightSpr;
                bodyText.text = "You've Got " + Base._instance.randomRewardCount + " Light Hint Tips";
                break;
            case 2:
                tipImg.sprite = undoSpr;
                bodyText.text = "You've Got " + Base._instance.randomRewardCount + " Undo Block Tips";
                break;
        }
    }
}
