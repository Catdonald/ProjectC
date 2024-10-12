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

    public void UpdateHUD()
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
                   myslider.value = (float)GameManager.instance.UpgradeCount / GameManager.instance.upgradables.Count;
                }
                break;
        }
    }
}
