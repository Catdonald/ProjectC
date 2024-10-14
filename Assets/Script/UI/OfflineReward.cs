using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OfflineReward : MonoBehaviour
{
    [SerializeField] private Text reward;
    [SerializeField] private GameObject money;
    [SerializeField] private Transform movePos;
    [SerializeField] private GameObject[] moveMoney;
    public int rewardNow;
    private void Start()
    {
        Bounce();
    }
    public void SetRewardText(int money)
    {
        rewardNow = money;
        reward.text = GameManager.instance.GetFormattedMoney(money);
    }
    public void ReceiveReward()
    {       
        foreach(var money in moveMoney)
        {
            money.SetActive(true);
            money.transform.DOMove(movePos.position, 0.5f).OnComplete(() => { money.SetActive(false); });
            money.transform.DOScale(new Vector3(0.7f, 0.7f), 0.5f);
            money.transform.DORotate(new Vector3(0, 0, 13.277f), 0.5f);
        }

        gameObject.SetActive(false);
        GameManager.instance.SoundManager.PlaySFX("SFX_cashBell");
    }

    private void Bounce()
    {
        money.transform.DOScale(1.5f, 0.8f).OnComplete(() =>
        {
            money.transform.DOScale(1.3f, 0.8f).OnComplete(() =>
            {
                Bounce();
            });
        });
    }
}
