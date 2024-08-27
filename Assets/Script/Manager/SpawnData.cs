using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnData : MonoBehaviour
{
    public static SpawnData instance;
    void Awake()
    {
        instance = this;
    }

    public Data[] datas;
}

[System.Serializable]
public class Data
{
    public int maxSpawnCount;
    public int spawnSpeed;
}