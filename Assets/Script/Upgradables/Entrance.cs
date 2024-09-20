using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : Upgradable
{
    [SerializeField] private MeshRenderer[] entranceMeshes;
    [SerializeField] private BoxCollider entranceColli;

    // Update is called once per frame
    void Update()
    {

    }

    protected override void UpgradeStats()
    {
        entranceColli.enabled = false;

        foreach(MeshRenderer mesh in entranceMeshes)
        {
            mesh.enabled = true;
        }
    }
}
