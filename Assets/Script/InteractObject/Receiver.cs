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
    private bool isDeleting = false;

    void Awake()
    {
        stack = new Stack<GameObject>();
    }
    void Update()
    {
        if (stack.Count > 0 && !isDeleting)
        {
            StartCoroutine(DeleteObject());
        }
    }

    private IEnumerator DeleteObject()
    {
        isDeleting = true;

        yield return new WaitForSeconds(5f);

        GameManager.instance.PoolManager.Return(stack.Pop());

        isDeleting = false;
    }

    public void ReceiveObject(GameObject obj, float objectHeight)
    {
        this.stack.Push(obj);
        obj.GetComponent<Stuff>().UpdateObjectPosition(null, false);

        Vector3 pos = gameObject.transform.position;
        pos.y = pos.y + objectHeight * stack.Count;

        obj.transform.position = pos;
        obj.transform.SetParent(gameObject.transform);
    }

}
