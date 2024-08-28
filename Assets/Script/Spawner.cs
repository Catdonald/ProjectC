using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 240822 오수안
/// 버거를 생성하고 플레이어에게 주는 오브젝트 유형
/// </summary>
public class Spawner : MonoBehaviour
{
    bool isSpawning;
    public Stack<GameObject> burgerStack;

    [Header("spawn info")]
    public int foodType;
    public float foodHeight;
    public int countMax;
    public int countNow;
    public float spawnSpeed;

    void Awake()
    {
        isSpawning = false;
        burgerStack = new Stack<GameObject>();

        foodType = 0;
        countMax = 3;
        spawnSpeed = 3f;
    }
    void OnEnable()
    {
        GameObject food = GameManager.instance.PoolManager.Get(foodType);
        foodHeight = food.GetComponent<Renderer>().bounds.size.y;
        GameManager.instance.PoolManager.Return(food);
    }

    void Start()
    {

    }

    void Update()
    {
        if (burgerStack.Count < countMax && !isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnFood(foodType));
        }
    }

    IEnumerator SpawnFood(int index)
    {
        yield return new WaitForSeconds(spawnSpeed);

        GameObject food = GameManager.instance.PoolManager.Get(index);

        food.transform.position = 
            transform.position + Vector3.up * foodHeight * burgerStack.Count;

        burgerStack.Push(food);

        isSpawning = false;
    }

}
