using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : LevelableObject
{
    public float moveSpeed;
    public int maxCapacity;
    void Start()
    {
        moveSpeed = GameManager.instance.playerLevelData[Level].moveSpeed;
        maxCapacity = GameManager.instance.playerLevelData[Level].maxCapacity;
    }
    public override void LevelUp()
    {
        base.LevelUp();

        moveSpeed = GameManager.instance.playerLevelData[Level].moveSpeed;
        maxCapacity = GameManager.instance.playerLevelData[Level].maxCapacity;
    }
    public override void AddExperience(int amount)
    {
        base.AddExperience(amount);
    }

    // TODO ) ������ ��� ��� �� �ϴ���
}
