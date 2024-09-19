using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradable : MonoBehaviour
{
    [SerializeField] private static int objectCount = 0;
    
    [SerializeField] private Vector3 buyingPosition = Vector3.zero;
    protected int upgradeLevel = 0;

    //private List<UpgradeMesh> upgradeMeshes;
    public Vector3 BuyingPosition => transform.TransformPoint(buyingPosition);
    void Start()
    {

    }

    public virtual void Upgrade(bool effectOn = true)
    {
        upgradeLevel++;
        if (upgradeLevel > 1)
        {
            // 메쉬 업그레이드
        }
        else
        {
            gameObject.SetActive(true);
        }
        UpgradeStats();
        //unlock effect on
        if (!effectOn)
            return;
        // 업그레이드 된 메쉬 개수만큼 이펙트 재생


        // 게임 매니저의 다음 업그레이드 오브젝트를 변경
        GameManager.instance.SetNowUpgradableObject();
    }

    protected virtual void UpgradeStats() { }
}
