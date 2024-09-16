using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMachine : Upgradable
{
    [SerializeField] private float baseProductionInterval = 2.0f;
    [SerializeField] private int baseCapacity = 6;

    private Spawner spawner;

    private void Start()
    {
        spawner = GetComponentInChildren<Spawner>();
    }

    protected override void UpgradeStats()
    {
        spawner.actingTime = baseProductionInterval / upgradeLevel;
        spawner.MaxStackCount = baseCapacity * upgradeLevel;
    }
}