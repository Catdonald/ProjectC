using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : Upgradable
{
    [Header("unActive")]
    [SerializeField] private BoxCollider unActiveEntranceColli;

    protected override void UpgradeStats()
    {
        unActiveEntranceColli.enabled = false;
    }
}
