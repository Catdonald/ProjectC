using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    float stackSpeed;

    // test code
    Vector3 firstStackPos;
    Vector3 currentStackPos;

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
        stackSpeed = 0.1f;
    }

    public void OnEnterInteraction(Collision other)
    {
        Transform parentTransform = other.transform.parent;
        Spawner spawner = other.gameObject.transform.GetComponentInChildren<Spawner>();
        Receiver receiver = other.gameObject.transform.GetComponentInChildren<Receiver>();

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
        if (stackNow == 0 || 
            stackNow < stackMax ||
            stackNow != 0 && spawner.objectType == typeNow)
        {
            GameObject obj = spawner.RequestObject();

            if (obj != null)
            {
                stack.Push(obj);

                Vector3 pos = gameObject.transform.position;
                pos.y = pos.y + objectHeight * stack.Count;

                StartCoroutine(UpdateObjectPos(obj, pos, transform.parent.gameObject));
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

    private IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos, GameObject targetObject)
    {
        float elapsedTime = 0f;
        float duration = 0.2f;

        Vector3 startingPos = obj.transform.position;

        while (elapsedTime < duration)
        {
            targetPos.x = targetObject.transform.position.x;
            targetPos.z = targetObject.transform.position.z;
            obj.transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetPos.x = targetObject.transform.position.x;
        targetPos.z = targetObject.transform.position.z;
        obj.transform.position = targetPos;
    }
}
