using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 240822 오수안
/// 버거를 생성하고 플레이어에게 주는 오브젝트 유형
/// </summary>
public class Spawner : MonoBehaviour
{
    public SpawnData spawnData;

    bool isSpawning;
    public Stack<GameObject> stack;

    [Header("spawner info")]
    public int level;
    public int objectType;
    public float objectHeight;

    void Awake()
    {
        isSpawning = false;
        stack = new Stack<GameObject>();

        objectType = 0;
        level = 0;
    }

    void Start()
    {
        spawnData = SpawnData.instance;

        GameObject type = GameManager.instance.PoolManager.Get(objectType);
        objectHeight = type.GetComponent<Renderer>().bounds.size.y;
        GameManager.instance.PoolManager.Return(type);
    }

    void Update()
    {
        if (stack.Count < spawnData.datas[level].maxSpawnCount && !isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnObject(objectType));
        }
    }

    private IEnumerator SpawnObject(int index)
    {
        yield return new WaitForSeconds(spawnData.datas[level].spawnSpeed);

        GameObject obj = GameManager.instance.PoolManager.Get(index);

        obj.transform.position = 
            transform.position + Vector3.up * objectHeight * stack.Count;

        stack.Push(obj);

        isSpawning = false;
    }

    public GameObject RequestObject()
    {
        if(stack.Count > 0)
            return stack.Pop();
        else 
            return null;
    }

}