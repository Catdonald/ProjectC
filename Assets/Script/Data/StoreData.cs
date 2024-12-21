using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class StoreData
{
    public string StoreName {  get; set; }
    public long Money { get; set; }
    public int TutorialCount { get; set; }
    public int EmployeeSpeed { get; set; }
    public int EmployeeCapacity { get; set; }
    public int EmployeeAmount { get; set; }
    public int PlayerSpeed { get; set; }
    public int PlayerCapacity { get; set; }
    public int Profit { get; set; }

    public int UpgradeCount { get; set; }
    public int PaidAmount { get; set; }
    public bool IsUnlocked { get; set; } // Is All Objects are unlocked in the store

    public DateTime LastTime;

    public StoreData(string storeName, long money)
    {
        StoreName = storeName;
        Money = money;
    }
}
