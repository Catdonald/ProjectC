using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 240822 ������
/// �÷��̾��� ���Ÿ� �������� ������Ʈ ����
/// </summary>

public class Receiver : MonoBehaviour
{
    public Stack<GameObject> stack;
    public int objectType;

    void Awake()
    {
        stack = new Stack<GameObject>();
    }

    public void ReceiveObject(GameObject obj, float objectHeight)
    {
        this.stack.Push(obj);

        Vector3 pos = gameObject.transform.position;
        pos.y = objectHeight * stack.Count;

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
