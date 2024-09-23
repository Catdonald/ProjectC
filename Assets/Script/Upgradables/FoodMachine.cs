using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMachine : Upgradable
{
    [SerializeField] private float baseProductionInterval = 5.0f;
    [SerializeField] private int baseCapacity = 6;
    [SerializeField] private Spawner spawner;

    private void Start()
    {
        spawner = GetComponentInChildren<Spawner>();
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