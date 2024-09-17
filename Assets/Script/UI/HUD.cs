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
        /*switch (type)
        {
            case InfoType.Money:
                {
                    mytext.text = GameManager.instance.GetFormattedMoney();
                }
                break;
            case InfoType.LV:
                {
                    mytext.text = string.Format("LV {0:F0}", GameManager.instance.level);
                }
                break;
            case InfoType.EXP:
                {
                    int curEXP = GameManager.instance.exp;
                    int maxEXP = GameManager.instance.nextEXP[GameManager.instance.level];

                    myslider.value = curEXP / maxEXP;
                }
                break;
        }*/
    }
}
