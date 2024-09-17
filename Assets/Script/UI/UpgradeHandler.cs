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

    private Color halfActiveColor = new Color(1.0f, 0.5f, 0.25f, 0.5f);
    private Color activeColor = new Color(1.0f, 0.5f, 0.25f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        upgradeButton.onClick.AddListener(() =>
            GameManager.instance.PurchaseUpgrade(upgradeType)
        );
        GameManager.instance.OnUpgrade += UpdateHandler;
    }

    private void UpdateHandler()
    {
        // TODO
        // 레벨에 따른 인디케이터 활성화
        // priceLabel Text 업데이트
        //upgradeButton.interactable = bool;
    }
}
