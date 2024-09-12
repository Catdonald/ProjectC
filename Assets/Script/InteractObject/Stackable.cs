using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour, IStackObject
{
    public playerStack player;
    public float actingTime { get; set; } = 0;
    public bool isActing { get; set; } = false;
    public eFurnitureType f_type { get; set; } = eFurnitureType.LAST;
    public eObjectType type { get; set; } = eObjectType.LAST;
    public Stack<GameObject> stack { get; set; } = new Stack<GameObject>();
    public float objectHeight { get; set; } = 0;
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
    public virtual IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos)
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
