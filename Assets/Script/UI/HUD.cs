using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Head up display (UI System)
/// </summary>

public class HUD : MonoBehaviour
{
    public enum InfoType { Money, LV, EXP }
    public InfoType type;

    Text mytext;
    Slider myslider;

    void Awake()
    {
        myslider = GetComponent<Slider>();
        mytext = GetComponent<Text>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Money:
                {
                    mytext.text = GameManager.instance.GetFormattedMoney(GameManager.instance.data.Money);
                }
                break;
            case InfoType.LV:
                {
                    mytext.text = string.Format("{0:0}%", (float)GameManager.instance.UpgradeCount / GameManager.instance.upgradables.Count * 100);
                }
                break;
            case InfoType.EXP:
                {
                    int curEXP = GameManager.instance.data.EXP;
                    int maxEXP = GameManager.instance.data.NextEXP;

                    myslider.value = curEXP / maxEXP;
                }
                break;
        }
    }
}
