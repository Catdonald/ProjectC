using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHandler : MonoBehaviour
{
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text priceLabelText;
    [SerializeField] private Image[] indicators;

    private List<Color> activeColors = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        activeColors.Add(new Color(1.0f, 0.5f, 0.25f, 0.5f));
        activeColors.Add(new Color(1.0f, 0.5f, 0.25f, 1.0f));
        upgradeButton.onClick.AddListener(() =>
            GameManager.instance.PurchaseUpgrade(upgradeType)
        );
        GameManager.instance.OnUpgrade += UpdateHandler;
    }

    private void UpdateHandler()
    {
        int currentLevel = GameManager.instance.GetUpgradeLevel(upgradeType);
        for(int i = 0; i < indicators.Length; i++)
        {
            indicators[i].color = i < currentLevel ? (currentLevel / 5 >= 1 && (currentLevel % 5 >= (i + 1)) ? activeColors[1] : activeColors[0]) : Color.gray;
        }

        if(currentLevel < 10)
        {
            int price = GameManager.instance.GetUpgradePrice(upgradeType);
            priceLabelText.text = GameManager.instance.GetFormattedMoney(price);
            upgradeButton.interactable = GameManager.instance.GetMoney() >= price;
        }
        else
        {
            priceLabelText.text = "MAX";
            upgradeButton.interactable = false;
        }
    }
}
