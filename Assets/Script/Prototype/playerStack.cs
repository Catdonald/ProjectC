using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 240822 오수안
/// 플레이어가 이고 다니는 버거 스택
/// </summary>

public class playerStack : MonoBehaviour
{
    public Spawner spawner;
    public Receiver receiver;

    public Stack<GameObject> stack;
    float objectHeight;

    bool isInArea;

    [Header("player stack info")]
    int typeNow;
    int stackMax;
    int stackNow;

    int stackTime;

    void Awake()
    {
        isInArea = false;
        stack = new Stack<GameObject>();

        spawner = null;
        receiver = null;

        objectHeight = 0;

        typeNow = 0;
        stackMax = 3;
        stackNow = 0;
    }

    public void OnEnterInteraction(Collider other)
    {
        Transform parentTransform = other.transform.parent;
        Spawner spawner = parentTransform.GetComponentInChildren<Spawner>();
        Receiver receiver = parentTransform.GetComponentInChildren<Receiver>();

        if (spawner != null)
        {
            this.spawner = spawner;
            this.objectHeight = spawner.objectHeight;
        }
        else if (receiver != null)
        {
            this.receiver = receiver;
        }

        isInArea = true;
    }

    public void InteractWithSpawner()
    {
        if (stackNow == 0 || stackNow != 0 && spawner.objectType == typeNow)
        {
            GameObject obj = spawner.RequestObject();

            if (obj != null)
            {
                stack.Push(obj);

                Vector3 pos = gameObject.transform.position;
                pos.y = pos.y + objectHeight * stack.Count;

                obj.transform.position = pos;
                obj.transform.SetParent(gameObject.transform);
            }
        }
    }
    public void InteractWithReceiver()
    {
        if (stack.Count > 0 && receiver.objectType == typeNow)
            receiver.ReceiveObject(stack.Pop(), objectHeight);
    }

    public void OnExitInteraction()
    {
        spawner = null;
        receiver = null;
        isInArea = false;
    }
}
