using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBox : Interactable
{
    [Header("Button info")]
    [SerializeField] private float payingInterval = 0.01f;
    [SerializeField] private float payingTime = 0.4f;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI priceText;

    private int upgradePrice;
    private int paidAmount;

    private long playerMoney => GameManager.instance.GetMoney();

    protected override void OnPlayerEnter()
    {
        StartCoroutine(Filling());
    }

    protected override void OnPlayerExit()
    {

    }

    public void Initialize(int upgradePrice, int paidAmount)
    {
        this.upgradePrice = upgradePrice;
        this.paidAmount = paidAmount;
        UpdatePayAmount(0);
    }

    private void UpdatePayAmount(int value)
    {
        paidAmount += value;
        GameManager.instance.PaidAmount = paidAmount;
        fill.fillAmount = (float)paidAmount / upgradePrice;
        priceText.text = GameManager.instance.GetFormattedMoney(upgradePrice - paidAmount);
    }

    IEnumerator Filling()
    {
        yield return new WaitForSeconds(2.0f);
        if (player != null && paidAmount < upgradePrice && playerMoney > 0)
        {
            GameManager.instance.SoundManager.PlayPitchSound("SFX_money");
            StartCoroutine(PlayFillingStartAnimation());
        }

        while (player != null && paidAmount < upgradePrice && playerMoney > 0)
        {
            float paymentRate = upgradePrice * payingInterval / payingTime;
            paymentRate = Mathf.Min(playerMoney, paymentRate);
            int payment = Mathf.Max(1, Mathf.RoundToInt(paymentRate));
            if(paidAmount + payment > upgradePrice)
            {
                payment = upgradePrice - paidAmount;
            }
            UpdatePayAmount(payment);
            GameManager.instance.AdjustMoney(-payment);

            Vibration.Vibrate(200);
            PlayMoneyAnimation();

            if (paidAmount >= upgradePrice)
            {
                GameManager.instance.SoundManager.QuitPitchSound();
                GameManager.instance.BuyUpgradable();
            }
            yield return new WaitForSeconds(payingInterval);
        }
        GameManager.instance.SoundManager.QuitPitchSound();
        Vibration.Cancel();
    }

    IEnumerator PlayFillingStartAnimation()
    {
        int count = 0;
        Vector3 targetPos = transform.position;
        while (count < 3)
        {
            GameObject moneyObj = GameManager.instance.PoolManager.SpawnObject("Money");
            moneyObj.transform.SetParent(null);
            moneyObj.transform.position = player.transform.position - Vector3.up * 0.5f;
            moneyObj.transform.rotation = player.playerRoot.transform.rotation;
            moneyObj.transform.DOJump(targetPos, 2.5f, 1, 0.1f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                GameManager.instance.PoolManager.Return(moneyObj);
            });
            ++count;
            yield return new WaitForSeconds(payingInterval);
        }
    }

    private void PlayMoneyAnimation()
    {
        GameObject moneyObj = GameManager.instance.PoolManager.SpawnObject("Money");
        moneyObj.transform.SetParent(null);
        moneyObj.transform.position = player.transform.position - Vector3.up * 0.5f;
        moneyObj.transform.rotation = player.playerRoot.transform.rotation;
        Vector3 targetPos = transform.position;
        moneyObj.transform.DOJump(targetPos, 2.5f, 1, 0.1f).SetEase(Ease.OutQuad)
        .OnComplete(() =>
        {
            moneyObj.transform.rotation = Quaternion.identity;
            GameManager.instance.PoolManager.Return(moneyObj);
        });
    }
}
