using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour, IStackObject
{
    public playerStack player;
    public float actingTime { get; set; } = 0;
    public bool isActing { get; set; } = false;

    public eObjectType type;
    public Stack<GameObject> stack { get; set; } = new Stack<GameObject>();
    public float objectHeight { get; set; } = 0;
    public int Count => stack.Count;
    public int MaxStackCount { get; set; } = 5;
    public bool IsFull => stack.Count >= MaxStackCount;

    public Vector3 PeekPoint => transform.position + new Vector3(0, objectHeight * Count, 0);
    
    public virtual void Enter(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;
    }
    public virtual void Interaction(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;
    }
    public virtual void Exit()
    {
        player = null;
    }
    public virtual void ReceiveObject(GameObject obj, eObjectType objType, float objHegiht)
    {
        if (IsFull) return;
        this.stack.Push(obj);
        this.objectHeight = objHegiht;

        Vector3 pos = gameObject.transform.position;
        pos.y = pos.y + objectHeight * (stack.Count - 1);

        StartCoroutine(UpdateObjectPos(obj, pos, gameObject));
        obj.transform.SetParent(gameObject.transform);
    }
    public virtual GameObject RequestObject()
    {
        return stack.Pop();
    }
    public virtual IEnumerator SpawnObject(int type)
    {
        yield return new WaitForSeconds(actingTime);

        GameObject obj = GameManager.instance.PoolManager.Get(type);

        obj.transform.position =
            transform.position + Vector3.up * objectHeight * stack.Count;

        stack.Push(obj);

        isActing = false;
    }
    public virtual IEnumerator DestryObject()
    {
        yield return new WaitForSeconds(actingTime);
    }
    public virtual IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos, GameObject targetObject)
    {
        float elapsedTime = 0f;
        float duration = 0.2f;

        Vector3 startingPos = obj.transform.position;

        while (elapsedTime < duration)
        {
            // 목표 지점의 X, Z 좌표를 targetObject의 위치로 업데이트
            targetPos.x = targetObject.transform.position.x;
            targetPos.z = targetObject.transform.position.z;

            // 기본적인 Lerp로 X, Z 축 이동
            Vector3 newPosition = Vector3.Lerp(startingPos, targetPos, elapsedTime / duration);

            // Y 축에 점프 효과 추가 (포물선 모양)
            float normalizedTime = elapsedTime / duration;
            newPosition.y += Mathf.Sin(normalizedTime * Mathf.PI) * 1;

            // 오브젝트 위치 갱신
            obj.transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetPos.x = targetObject.transform.position.x;
        targetPos.z = targetObject.transform.position.z;
        obj.transform.position = targetPos;
    }
    public virtual IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos)
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
    }
}
