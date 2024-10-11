using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : Upgradable
{
    public override void UpgradeStats()
    {
        GameManager.instance.upgradableCam.waitDuration = 3.0f;
        FindObjectOfType<PlayerController>().Animator.SetTrigger("unlockEntrance");
    }
}
