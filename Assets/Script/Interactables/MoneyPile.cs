using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPile : ObjectPile
{
    [SerializeField] private int maxPile = 120;
    [SerializeField, Range(1, 8)] private int collectMultiplier = 2;
    private int hiddenMoney;
    private bool isCollectingMoney;
    private int collectRate => objects.Count > 8 ? collectMultiplier : 1;

    protected override void Drop()
    {
        if (!isCollectingMoney)
        {
            return;
        }

        GameObject moneyObj = GameManager.instance.PoolManager.SpawnObject("Money");
        moneyObj.transform.position = objects.Peek().transform.position;
        moneyObj.transform.DOJump(player.transform.position + Vector3.up * 2, 3.0f, 1, 0.5f)
            .OnComplete(() => GameManager.instance.PoolManager.Return(moneyObj));
    }

    protected override void OnPlayerEnter()
    {
        StartCoroutine(CollectMoney());
    }

    IEnumerator CollectMoney()
    {
        isCollectingMoney = true;
        GameManager.instance.AdjustMoney(hiddenMoney);
        hiddenMoney = 0;
        GameManager.instance.SoundManager.PlayPitchSound("SFX_money");

        while (player != null && objects.Count > 0)
        {
            for (int i = 0; i < collectRate; i++)
            {
                if (objects.Count == 0)
                {
                    isCollectingMoney = false;
                    break;
                }

                GameObject collectedMoney = objects.Pop();
                GameManager.instance.PoolManager.Return(collectedMoney);
                GameManager.instance.AdjustMoney(1);
            }
            if (collectRate > 1)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(0.03f);
            }
        }

        isCollectingMoney = false;
        GameManager.instance.SoundManager.QuitPitchSound();
    }

    public void AddMoney()
    {
        if (objects.Count < maxPile)
        {
            GameObject moneyObj = GameManager.instance.PoolManager.SpawnObject("Money");
            AddObject(moneyObj);
        }
        else
        {
            hiddenMoney++;
        }

        if (!isCollectingMoney && player != null)
        {
            StartCoroutine(CollectMoney());
        }
    }
}
