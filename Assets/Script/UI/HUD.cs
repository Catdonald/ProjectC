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
                    mytext.text = string.Format("LV {0:F0}", GameManager.instance.data.Level);
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
