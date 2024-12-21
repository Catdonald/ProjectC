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

        if (type == eObjectType.BURGERPACK)
        {
            if (player.Count == player.Capacity)
            {
                return;
            }
        }
        else if (type == eObjectType.TRASH)
        {
            if (player.Count == 50)
            {
                return;
            }
        }
        else if (type == eObjectType.EMPTYCUP)
        {
            if (player.Count == 50)
            {
                return;
            }
        }

        if (player.StackType == eObjectType.LAST || player.StackType == type)
        {         
            player.AddToStack(stack.Pop(), type);
            Vibration.Vibrate(100);
            GameManager.instance.SoundManager.PlaySFX("SFX_stack");
            if (linkedObject != null && stack.Count == 0)
                linkedObject.SetActive(false);
        }
    }

    public void RemoveAndStackObject(playerStack playerStack)
    {
        var removeObj = stack.Pop();
        if(linkedObject != null && stack.Count == 0)
            linkedObject.SetActive(false);
        playerStack.AddToStack(removeObj, type);
    }
}
