using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelable
{
    int Level { get; set; }
    int Experience { get; set; }

    void LevelUp();
    void AddExperience(int amount);
}
