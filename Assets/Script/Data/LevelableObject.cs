using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelableObject : MonoBehaviour, ILevelable
{
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int experienceToNextLevel = 100;

    public virtual void LevelUp()
    {
        Level++;
        Experience = 0;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.2f); // 레벨업에 필요한 경험치 증가
        Debug.Log($"{gameObject.name} leveled up to {Level}!");
    }

    public virtual void AddExperience(int amount)
    {
        Experience += amount;
        if (Experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
}
