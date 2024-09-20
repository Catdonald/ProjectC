using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradable : MonoBehaviour
{   
    [SerializeField] private Vector3 buyingPosition = Vector3.zero;
    protected int upgradeLevel = 0;

    //private List<UpgradeMesh> upgradeMeshes;
    public Vector3 BuyingPosition => transform.TransformPoint(buyingPosition);
    
    protected void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void Upgrade(bool effectOn = true)
    {
        upgradeLevel++;
        if (upgradeLevel > 1)
        {
            // �޽� ���׷��̵�
        }
        else
        {
            gameObject.SetActive(true);
        }
        UpgradeStats();
        //unlock effect on
        if (!effectOn)
            return;
        // ���׷��̵� �� �޽� ������ŭ ����Ʈ ���
    }

    protected virtual void UpgradeStats() { }
}
