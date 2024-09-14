using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 240822 오수안
/// 플레이어의 버거를 가져가는 오브젝트 유형
/// </summary>

public class Receiver : MonoBehaviour
{
    public Stack<GameObject> stack;
    public StackType objectType;
    private float _objectHeight;
    public float objectHeight
    {
        get { return _objectHeight; }
        set { _objectHeight = value; }
    }
    public int MaxStackCount { get; set; }
    public bool IsFull => stack.Count >= MaxStackCount;

    void Awake()
    {
        stack = new Stack<GameObject>();

        // 임시
        for(int i = 0; i < 90; ++i)
        {
            stack.Push(new GameObject());
        }
        MaxStackCount = 100;
    }

    public void ReceiveObject(GameObject obj, float objHeight)
    {
        this.stack.Push(obj);
        this._objectHeight = objHeight;

        Vector3 pos = gameObject.transform.position;
        pos.y = pos.y + _objectHeight * (stack.Count - 1);

        StartCoroutine(UpdateObjectPos(obj, pos));
        obj.transform.SetParent(gameObject.transform);
    }

    private IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos)
    {
        float elapsedTime = 0f;
        float duration = 0.1f;

        Vector3 startingPos = obj.transform.position;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPos;
    }
    public GameObject CustomerRequest()
    {
        return stack.Pop();
    }

}
