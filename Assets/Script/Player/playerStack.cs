using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

/// <summary>
/// 240822
/// 플레이어, 직원 스택
/// </summary>


public class playerStack : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Vector2 rateRange = new Vector2(0.8f, 0.4f);
    [SerializeField] private float bendFactor = 0.1f;
    public int Height { get; private set; }
    public int Count => stack.Count;
    public eObjectType StackType { get; private set; }
    public bool IsFull => stack.Count >= Capacity;
    public int Capacity = 5;
    private List<GameObject> stack = new List<GameObject>();


    private float StackOffset => GameManager.instance.GetStackOffset(StackType);

    void Start()
    {
        StackType = eObjectType.LAST;
    }

    void Update()
    {
        if (stack.Count == 0 || !playerController)
        {
            return;
        }

        stack[0].transform.position = transform.position;
        stack[0].transform.rotation = transform.rotation;

        for (int i = 1; i < stack.Count; i++)
        {
            float rate = Mathf.Lerp(rateRange.x, rateRange.y, i / (float)stack.Count);
            stack[i].transform.position = Vector3.Lerp(stack[i].transform.position,
                stack[i - 1].transform.position + (stack[i - 1].transform.up * StackOffset), rate);
            stack[i].transform.rotation = Quaternion.Lerp(stack[i].transform.rotation,
                stack[i - 1].transform.rotation, rate);
            if (playerController.mouseDelta != Vector3.zero)
            {
                stack[i].transform.rotation *= Quaternion.Euler(-i * bendFactor * rate, 0, 0);
            }
        }
    }

    public void AddToStack(GameObject obj, eObjectType objType)
    {
        if (stack.Count == 0)
        {
            StackType = objType;
        }

        Vector3 peakPoint = transform.position + Vector3.up * Height * StackOffset;
        Height++;

        obj.transform.DOJump(peakPoint, 5f, 1, 0.3f)
            .OnComplete(() =>
            {
                stack.Add(obj);
                obj.transform.SetParent(transform);
            });
    }

    public GameObject RemoveFromStack()
    {
        if (stack.Count == 0)
        {
            return null;
        }

        var removeObj = stack.LastOrDefault();
        removeObj.transform.rotation = Quaternion.identity;
        stack.Remove(removeObj);
        Height--;
        if (stack.Count == 0)
        {
            StackType = eObjectType.LAST;
        }
        return removeObj;
    }
}
