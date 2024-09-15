using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;

public class StaffData : LevelableObject
{
    public float moveSpeed;
    public int maxCapacity;
    void Start()
    {
        moveSpeed = GameManager.instance.staffLevelData[Level].moveSpeed;
        maxCapacity = GameManager.instance.staffLevelData[Level].maxCapacity;
    }
    public override void LevelUp()
    {
        base.LevelUp();

        moveSpeed = GameManager.instance.staffLevelData[Level].moveSpeed;
        maxCapacity = GameManager.instance.staffLevelData[Level].maxCapacity;
    }
    public override void AddExperience(int amount)
    {
        base.AddExperience(amount);
    }
}
