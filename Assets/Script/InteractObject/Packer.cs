using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Packer : MonoBehaviour
{
    [SerializeField] private Receiver burgerReceiver;
    [SerializeField] private Giver PackGiver;

    private bool isWorkerIn;
    private float boxHeight;
    private int index = 0;
    private float elapsedtime = 0;
    private float packingtime = 3;
    [SerializeField] private GameObject box;
    [SerializeField] private Transform[] foodPosition;

    void Start()
    {
        GameObject height = GameManager.instance.PoolManager.Get(3);
        boxHeight = height.GetComponent<Renderer>().bounds.size.y;
        GameManager.instance.PoolManager.Return(height);
    }

    void Update()
    {
        Packing();
    }

    void Packing()
    {
        if (burgerReceiver.stack.Count == 0)
        {
            elapsedtime = 0;
            return;
        }

        if (box == null)
        {
            box = GameManager.instance.PoolManager.Get(3);
            box.transform.position = transform.position;
        }

        if (elapsedtime < packingtime)
        {
            elapsedtime += Time.deltaTime;
        }
        else
        {
            PosBurger();
            elapsedtime = 0;
        }
    }

    void PosBurger()
    {
        GameObject burger = burgerReceiver.stack.Pop();

        StartCoroutine(UpdateObjectPos(burger, foodPosition[index].position));
        burger.transform.parent = box.transform;
    }
    IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos)
    {
        float elapsedTime = 0f;
        float duration = 0.2f;

        Vector3 startingPos = obj.transform.position;

        while (elapsedTime < duration)
        {
            // 기본적인 Lerp로 X, Z 축 이동
            Vector3 newPosition = Vector3.Lerp(startingPos, targetPos, elapsedTime / duration);

            // Y 축에 점프 효과 추가 (포물선 모양)
            float normalizedTime = elapsedTime / duration;
            newPosition.y += Mathf.Sin(normalizedTime * Mathf.PI) * 1;

            // 오브젝트의 위치를 갱신
            obj.transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPos;

        ++index;
        if (index == 4)
        {
            PackGiver.ReceiveObject(box, eObjectType.BURGERPACK, boxHeight);
            index = 0;
            box = null;
        }
    }
}
