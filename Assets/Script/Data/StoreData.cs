using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class StoreData
{
    public string StoreName {  get; set; }
    public int Level { get; set; }
    public int EXP { get; set; }
    public int NextEXP { get; set; }
    public long Money { get; set; }

    public int EmployeeSpeed { get; set; }
    public int EmployeeCapacity { get; set; }
    public int EmployeeAmount { get; set; }
    public int PlayerSpeed { get; set; }
    public int PlayerCapacity { get; set; }
    public int Profit { get; set; }

    public int UnlockCount { get; set; }
    public int PaidAmount { get; set; }
    public bool IsUnlocked { get; set; } // Is All Objects are unlocked in the store

    public StoreData(string storeName, long money, int EXP, int Level)
    {
        StoreName = storeName;
        Money = money;
        NextEXP = EXP;
        this.Level = Level;
    }
}
