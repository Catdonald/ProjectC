using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어나 직원이 일하는 오브젝트
/// </summary>
public class WorkStation : Upgradable
{
    [SerializeField] private GameObject worker;
    [SerializeField] private WorkingSpot workingSpot;
    public bool HasWorker => upgradeLevel > 1 ? true : workingSpot.HasWorker;
    public EmployeeController WorkingEmployee => workingSpot.WorkingEmployee;
    public Vector3 WorkingSpotPosition => workingSpot.transform.position;

    public override void Upgrade(bool effectOn = true)
    {
        base.Upgrade(effectOn);
        worker.SetActive(upgradeLevel > 1);
        workingSpot.gameObject.SetActive(upgradeLevel <= 1);
    }
}
