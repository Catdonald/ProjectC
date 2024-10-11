using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : Upgradable
{
    private PlayerController playerController;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    public override void UpgradeStats()
    {
        
    }
}
