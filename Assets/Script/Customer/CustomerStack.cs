using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStack : MonoBehaviour
{
    private Stack<GameObject> stack;
    private StackType stackType;

    public int Count => stack.Count;

    void Awake()
    {
        stack = new Stack<GameObject>();
    }

    public void ReceiveObject(GameObject obj, float objHeight)
    {
        stack.Push(obj);

        Vector3 pos = gameObject.transform.position;
        pos.y = pos.y + objHeight * (stack.Count - 1);

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

    public GameObject RemoveFromStack()
    {
        if(stack.Count == 0)
            return null;
        GameObject obj = stack.Pop();
        if(stack.Count == 0)
        {
            stackType = StackType.NONE;
        }
        return obj;
    }

}
