using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBox : Interactable
{
    [Header("Button info")]
    public int maxPrice;
    public int curPrice;
    public int width;
    public int height;
    public bool isPushed = false;
    public bool isFilling = false;

    [Header("Image obj")]
    [SerializeField] private Image background;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI priceText;

    // Start is called before the first frame update
    void Start()
    {
        maxPrice = 100;
        curPrice = 0;
        fill.fillAmount = 0;
        isPushed = false;
        priceText.text = maxPrice.ToString();
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
        if (isPushed && GameManager.instance.GetMoney() > 0 && !isFilling)
        {
            StartCoroutine(Filling());
        }

        if (fill.fillAmount >= 1.0f)
        {
            isFilling = false;

            GameManager.instance.currentUpgradableObj.GetComponent<Upgradable>().Upgrade();
            maxPrice *= 2;
            GameManager.instance.SetNowUpgradableObject();
            SetOrigin();

            // TODO ) 카메라 이동 및 이펙트
        }
    }
    void SetOrigin()
    {
        curPrice = 0;
        fill.fillAmount = 0.0f;
        priceText.text = maxPrice.ToString();
    }
    IEnumerator Filling()
    {
        isFilling = true;

        while (isPushed && GameManager.instance.data.Money > 0 && curPrice < maxPrice && isFilling)
        {
            GameManager.instance.data.Money--;
            curPrice++;

            fill.fillAmount = (float)curPrice / (float)maxPrice;

            if (curPrice >= maxPrice)
            {
                curPrice = maxPrice;
                fill.fillAmount = 1f;
                isFilling = false;
                yield break;
            }

            yield return new WaitForSeconds(0.01f);
        }

        isFilling = false;
    }
}
