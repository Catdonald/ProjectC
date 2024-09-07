using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TableStack : MonoBehaviour
{
    public Stack<GameObject> stack;

    void Awake()
    {
        stack = new Stack<GameObject>();
    }

    public void ReceiveObject(GameObject obj, float objHeight)
    {
        stack.Push(obj);

        Vector3 pos = transform.position  + Vector3.up * objHeight * stack.Count;
        StartCoroutine(UpdateObjectPos(obj, pos));

        obj.transform.parent = null;
    }

    private IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos)
    {
        float elapsedTime = 0f;
        float duration = 0.2f;

        Vector3 startingPos = obj.transform.position;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPos;
    }
}
