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
    [SerializeField] private GameObject moveMoney;
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
        moveMoney.SetActive(true);
        moveMoney.transform.DOMove(movePos.position, 0.5f).OnComplete(() => { gameObject.SetActive(false); });
    }

    private void Bounce()
    {
        money.transform.DOScale(1.2f, 0.8f).OnComplete(() =>
        {
            money.transform.DOScale(1, 0.8f).OnComplete(() =>
            {
                Bounce();
            });
        });
    }
}
