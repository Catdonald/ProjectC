using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingStack : MonoBehaviour
{
    public int Count => stack.Count;
    private Stack<GameObject> stack = new Stack<GameObject>();
    private float height = 0f;

    public void AddToStack(GameObject obj, eObjectType type)
    {
        if (obj == null) return;
        float objHeight = GetStackOffset(type);
        Vector3 peekPoint = transform.position + Vector3.up * height;
        height += objHeight;
        obj.transform.DOJump(peekPoint, 5.0f, 1, 0.3f)
            .OnComplete(() =>
            {
                obj.transform.SetParent(transform);
                stack.Push(obj);
            });
    }

    public void RemoveAll()
    {
        if (stack.Count == 0) return;
        for (int i = 0; i < stack.Count; i++)
        {
            var removeObj = stack.Pop();
            GameManager.instance.PoolManager.Return(removeObj);
        }
        height = 0.0f;
    }

    private float GetStackOffset(eObjectType type)
    {
        if (type == eObjectType.HAMBURGER)
        {
            return 0.4f;
        }
        else
        {
            return 0.5f;
        }
    }
}
