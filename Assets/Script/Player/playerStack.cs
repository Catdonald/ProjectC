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

    [Header("player stack info")]
    int typeNow;
    int stackMax;
    float stackSpeed;

    void Awake()
    {
        stack = new Stack<GameObject>();

        spawner = null;
        receiver = null;

        objectHeight = 0;

        typeNow = 0;
        stackMax = 3;
        stackSpeed = 0.1f;
    }

    public void OnEnterInteraction(Collision other)
    {
        Transform parent = other.transform.parent;
        Spawner spawner = parent.GetComponentInChildren<Spawner>();
        Receiver receiver = parent.GetComponentInChildren<Receiver>();

        if (spawner != null)
        {
            this.spawner = spawner;
            this.objectHeight = spawner.objectHeight;
        }
        else if (receiver != null)
        {
            this.receiver = receiver;
        }
    }

    public void InteractWithSpawner()
    {
        if(spawner == null)
            return;

        if (spawner.stack.Count != 0 ||
            (spawner.stack.Count >= 1 &&
            spawner.objectType == typeNow))
        {
            GameObject obj = spawner.RequestObject();

            if (obj != null)
            {
                Vector3 pos = gameObject.transform.position;
                pos.y = pos.y + objectHeight * stack.Count;

                StartCoroutine(UpdateObjectPos(obj, pos, transform.parent.gameObject));
                obj.transform.SetParent(gameObject.transform);

                stack.Push(obj);
            }
            else
            {
                Debug.LogWarning("RequestObj() returned null. No object to process.");
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
    }

    private IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos, GameObject targetObject)
    {
        float elapsedTime = 0f;
        float duration = 0.1f;

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
