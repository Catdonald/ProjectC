using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableData : LevelableObject
{
    public float eatSpeed;
    public int price;
    void Start()
    {
        eatSpeed = GameManager.instance.tableLevelData[Level].eatSpeed;
        price = GameManager.instance.tableLevelData[Level].price;
    }
    public override void LevelUp()
    {
        base.LevelUp();

        eatSpeed = GameManager.instance.tableLevelData[Level].eatSpeed;
        price = GameManager.instance.tableLevelData[Level].price;
    }
    public override void AddExperience(int amount)
    {
        base.AddExperience(amount);
    }
}
