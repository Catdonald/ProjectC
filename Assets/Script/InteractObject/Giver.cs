using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giver : Stackable
{
    [SerializeField] GameObject linkedObject;

    void Start()
    {
        GameObject obj = GameManager.instance.PoolManager.SpawnObject("Burger_Package");
        objectHeight = obj.GetComponent<Renderer>().bounds.size.y;
        GameManager.instance.PoolManager.Return(obj);
        actingTime = 5;
    }
    public override void Enter(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;
        base.Enter(other);
        player = other.transform.GetComponentInChildren<playerStack>();
    }

    public override void Interaction(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        if (stack.Count == 0)
            return;

        if (player.stack.Count == 0 && player.type == eObjectType.LAST)
            player.type = type;

        if (player.type == type)
        {
            player.ReceiveObject(stack.Pop(), type, objectHeight);
            if (linkedObject != null && stack.Count == 0)
                linkedObject.SetActive(false);
        }
    }

    public void RemoveAndStackObject(playerStack playerStack)
    {
        var removeObj = stack.Pop();
        if(linkedObject != null && stack.Count == 0)
            linkedObject.SetActive(false);
        playerStack.ReceiveObject(removeObj, type, objectHeight);
    }
}
