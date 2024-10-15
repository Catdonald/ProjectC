using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMachine : Upgradable
{
    [SerializeField] private float baseProductionInterval = 5.0f;
    [SerializeField] private int baseCapacity = 6;
    [SerializeField] private Spawner spawner;
    [SerializeField] private GameObject maxImg;

    private void Start()
    {
        spawner = GetComponentInChildren<Spawner>();
    }
    private void Update()
    {
        if (!gameObject.activeSelf)
            return;

        if (spawner.IsFull && !maxImg.activeSelf)
            maxImg.SetActive(true);
        else if (!spawner.IsFull && maxImg.activeSelf)
            maxImg.SetActive(false);
    }

    public override void UpgradeStats()
    {
        spawner.actingTime = baseProductionInterval / upgradeLevel;
        spawner.MaxStackCount = baseCapacity * upgradeLevel;
    }
    void OnCollisionEnter(Collision collision)
    {
        spawner.Enter(collision);
    }
    void OnCollisionStay(Collision collision)
    {
        spawner.Interaction(collision);
    }
}