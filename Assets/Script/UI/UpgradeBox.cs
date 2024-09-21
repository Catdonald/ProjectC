using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBox : Interactable
{
    [Header("Button info")]
    public float Price;
    public float curPrice;
    public int width;  
    public int height;
    public bool isPushed = true;

    [Header("Image obj")]
    [SerializeField] private Image background;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI priceText;

    // Start is called before the first frame update
    void Start()
    {
        Price = 100;
        curPrice = 0;
        fill.fillAmount = 0;
        isPushed = false;
        priceText.text = Price.ToString();
    }
    protected override void OnPlayerEnter()
    {
        isPushed = true;
    }
    protected override void OnPlayerExit()
    {
        isPushed = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isPushed && GameManager.instance.GetMoney() > 0)
        {
            curPrice = GameManager.instance.GetMoney() / Price;
            StartCoroutine(Filling());
        }

        if (fill.fillAmount >= 1.0f)
        {
            GameManager.instance.currentUpgradableObj.GetComponent<Upgradable>().Upgrade();
            Price *= 1.2f;
            SetOrigin();
            GameManager.instance.SetNowUpgradableObject();

            // TODO ) 카메라 이동 및 이펙트
        }
    }
    void SetOrigin()
    {
        curPrice = 0;
        fill.fillAmount = 0.0f;
        priceText.text = Price.ToString();
    }
    IEnumerator Filling()
    {
        while (isPushed)
        {
            yield return new WaitForSeconds(0.1f);

            fill.fillAmount = Mathf.Lerp(fill.fillAmount, curPrice, 0.0001f);
        }
    }
}
