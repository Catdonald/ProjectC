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

        if (player == null)
            yield break;

        float paymentRate = upgradePrice * payingInterval / payingTime;

        // 플레이어의 돈이 한 프레임보다 작은경우
        if (playerMoney < paymentRate && playerMoney > 0)
        {
            paymentRate = playerMoney / 3;

            while (player != null && playerMoney > 0)
            {
                GameManager.instance.SoundManager.PlaySFX("SFX_money");
                paymentRate = Mathf.Min(playerMoney, paymentRate);
                int payment = Mathf.Max(1, Mathf.RoundToInt(paymentRate));

                UpdatePayAmount((int)paymentRate);
                GameManager.instance.AdjustMoney(-(int)paymentRate);

                GameObject moneyObj = GameManager.instance.PoolManager.SpawnObject("Money");
                moneyObj.transform.SetParent(null);
                moneyObj.transform.position = player.transform.position - Vector3.up * 0.5f;
                moneyObj.transform.DOJump(transform.position, 2.5f, 1, 0.3f).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    GameManager.instance.PoolManager.Return(moneyObj);
                });

                yield return new WaitForSeconds(0.02f);
            }
        }
        else
        {
            GameManager.instance.SoundManager.PlayPitchSound("SFX_money");
            while (player != null && paidAmount < upgradePrice && playerMoney > 0)
            {
                paymentRate = Mathf.Min(playerMoney, paymentRate);
                int payment = Mathf.Max(1, Mathf.RoundToInt(paymentRate));

                UpdatePayAmount(payment);
                GameManager.instance.AdjustMoney(-payment);

                Vibration.Vibrate(200);

                // money animation
                GameObject moneyObj = GameManager.instance.PoolManager.SpawnObject("Money");
                moneyObj.transform.SetParent(null);
                moneyObj.transform.position = player.transform.position - Vector3.up * 0.5f;
                moneyObj.transform.DOJump(transform.position, 2.5f, 1, 0.1f).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    GameManager.instance.PoolManager.Return(moneyObj);
                });

                if (paidAmount >= upgradePrice)
                {
                    GameManager.instance.SoundManager.QuitPitchSound();
                    GameManager.instance.BuyUpgradable();
                }
                yield return new WaitForSeconds(payingInterval);
            }
        }
        GameManager.instance.SoundManager.QuitPitchSound();
    }
}
