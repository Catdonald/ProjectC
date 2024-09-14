using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 240822 ������
/// ���Ÿ� �����ϰ� �÷��̾�� �ִ� ������Ʈ ����
/// </summary>
public class Spawner : MonoBehaviour
{
    public CookerData cookerData;

    bool isSpawning;
    public Stack<GameObject> stack;

    public StackType objectType;
    public float objectHeight;

    public int Count => stack.Count;

    void Awake()
    {
        cookerData = GetComponent<CookerData>();
        isSpawning = false;
        stack = new Stack<GameObject>();

        //objectType = StackType.NONE;
    }

    void Start()
    {
        GameObject type = GameManager.instance.PoolManager.Get((int)objectType);
        objectHeight = type.GetComponent<Renderer>().bounds.size.y;
        GameManager.instance.PoolManager.Return(type);
    }

    void Update()
    {
        if (stack.Count < cookerData.maxCapacity && !isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnObject((int)objectType));
        }
    }

    private IEnumerator SpawnObject(int index)
    {
        yield return new WaitForSeconds(cookerData.spawnSpeed);

        GameObject obj = GameManager.instance.PoolManager.Get(index);

        obj.transform.position = 
            transform.position + Vector3.up * objectHeight * stack.Count;

        stack.Push(obj);

        isSpawning = false;
    }

    public GameObject RequestObject()
    {
         return stack.Pop();
    }

}