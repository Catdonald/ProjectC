using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : Stackable
{
    void OnCollisionEnter(Collision collision)
    {
        player = collision.gameObject.GetComponentInChildren<playerStack>();
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            while (player.stack.Count > 0)
            {
                ReceiveObject(player.RequestObject(), eObjectType.TRASH, 0);
            }
        }
    }
    public override void ReceiveObject(GameObject obj, eObjectType objType, float objHegiht)
    {
        base.ReceiveObject(obj, objType, objHegiht);

        GameManager.instance.PoolManager.Return(obj);
    }
}
