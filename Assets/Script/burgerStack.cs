using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 240822 오수안
/// 플레이어가 이고 다니는 버거 스택
/// </summary>

public class burgerStack : MonoBehaviour
{
    public Stack<GameObject> playerStack;
    GameObject obj;
    Stack<GameObject> triggeredStack;
    float foodsize;

    bool isInArea;

    [Header("player stack info")]
    int typeNow;
    int stackMax;
    int stackNow;

    int stackTime;

    void Awake()
    {
        isInArea = false;

        playerStack = new Stack<GameObject>();
        triggeredStack = null;
        foodsize = 0;

        typeNow = 0;
        stackMax = 3;
        stackNow = 0;
    }

    public void OnEnterKitchen(int type, Collider other)
    {
        if (stackNow == 0 || stackNow != 0 && typeNow == type)
        {
            isInArea = true;
            triggeredStack = other.gameObject.transform.parent.GetComponentInChildren<Spawner>().burgerStack;
            foodsize = other.gameObject.transform.parent.GetComponentInChildren<Spawner>().foodHeight;
        }
        else
        {
            isInArea = false;
            triggeredStack = null;
        }
    }
    public void TryGetBurger()
    {
        if (!isInArea)
            return;

        if (triggeredStack.Count != 0)
        {
            var burger = triggeredStack.Pop();
            playerStack.Push(burger);
            Vector3 pos = transform.position;
            pos.y = pos.y / 2 + playerStack.Count * foodsize;
            burger.transform.position = pos;
            burger.transform.SetParent(this.transform);
        }
    }

    public void OnExitKitchen()
    {
        isInArea = false;
        triggeredStack = null;
    }

    public void OnEnterTable(int type, Collider other)
    {
        if (type == typeNow)
        {
            isInArea = true;
            triggeredStack = other.gameObject.transform.parent.GetComponentInChildren<table>().burgers;
            obj = other.gameObject;
        }
        else
        {
            isInArea = false;
        }
    }

    public void TrySetBurger()
    {
        if (playerStack.Count > 0)
        {
            var burger = playerStack.Pop();
            triggeredStack.Push(burger);

            Vector3 pos = obj.transform.position;
            pos.y = pos.y + foodsize * triggeredStack.Count;

            burger.transform.position = pos;
            burger.transform.SetParent(obj.transform);
        }
    }

    public void OnExitTable()
    {
        triggeredStack = null;
        isInArea = false;
    }


}
