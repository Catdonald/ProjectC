using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBox : Interactable
{
    [Header("Button info")]
    [SerializeField] private float payingInterval = 0.03f;
    [SerializeField] private float payingTime = 3f;
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
        GameManager.instance.SoundManager.QuitPitchSound();
        Vibration.Cancel();
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
        GameManager.instance.SoundManager.PlayPitchSound("SFX_money");
        while (player != null && paidAmount < upgradePrice && playerMoney > 0)
        {
            // 1원씩 지불하면 가격 비쌀수록 채우는데 오래 걸려서 보정함.
            float paymentRate = upgradePrice * payingInterval / payingTime;
            paymentRate = Mathf.Min(playerMoney, paymentRate);
            int payment = Mathf.Max(1, Mathf.RoundToInt(paymentRate));

            UpdatePayAmount(payment);
            GameManager.instance.AdjustMoney(-payment);

            Vibration.Vibrate(500);

            // money animation
            var moneyObj = GameManager.instance.PoolManager.SpawnObject("Money");
            moneyObj.transform.SetParent(null);
            moneyObj.transform.position = player.transform.position + Vector3.up * 1.0f;
            moneyObj.transform.DOJump(transform.position, 2.0f, 1, 0.2f)
                .OnComplete(() => GameManager.instance.PoolManager.Return(moneyObj));

            if (paidAmount >= upgradePrice)
            {
                GameManager.instance.SoundManager.QuitPitchSound();
                GameManager.instance.BuyUpgradable();
            }
            yield return new WaitForSeconds(payingInterval);
        }
        GameManager.instance.SoundManager.QuitPitchSound();
    }
}
