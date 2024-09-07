using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookerData : LevelableObject
{
    public float spawnSpeed;
    public int maxCapacity;

    void Start()
    {
        spawnSpeed = GameManager.instance.cookerLevelData[Level].spawnSpeed;
        maxCapacity = GameManager.instance.cookerLevelData[Level].maxCapacity;
    }
    public override void LevelUp()
    {
        base.LevelUp();

        spawnSpeed = GameManager.instance.cookerLevelData[Level].spawnSpeed;
        maxCapacity = GameManager.instance.cookerLevelData[Level].maxCapacity;
    }
    public override void AddExperience(int amount)
    {
        base.AddExperience(amount);
    }
}
