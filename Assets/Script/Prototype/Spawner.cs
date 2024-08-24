using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 240822 ������
/// ���Ÿ� �����ϰ� �÷��̾�� �ִ� ������Ʈ ����
/// </summary>
public class Spawner : MonoBehaviour
{
    bool isSpawning;
    public Stack<GameObject> stack;

    [Header("spawn info")]
    public int objectType;
    public float objectHeight;
    public int countMax;
    public int countNow;
    public float spawnSpeed;

    void Awake()
    {
        isSpawning = false;
        stack = new Stack<GameObject>();

        objectType = 0;
        countMax = 3;
        spawnSpeed = 3f;
    }
    void OnEnable()
    {
        GameObject food = GameManager.instance.PoolManager.Get(objectType);
        objectHeight = food.GetComponent<Renderer>().bounds.size.y;
        GameManager.instance.PoolManager.Return(food);
    }

    void Start()
    {

    }

    void Update()
    {
        if (stack.Count < countMax && !isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnObject(objectType));
        }
    }

    private IEnumerator SpawnObject(int index)
    {
        yield return new WaitForSeconds(spawnSpeed);

        GameObject food = GameManager.instance.PoolManager.Get(index);

        food.transform.position = 
            transform.position + Vector3.up * objectHeight * stack.Count;

        stack.Push(food);

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
