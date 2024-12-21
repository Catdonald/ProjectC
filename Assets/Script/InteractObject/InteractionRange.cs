using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRange : MonoBehaviour
{
    public enum eRangeType { SPAWNER, RECEIVER, GIVER }
    public eRangeType rangeType;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        switch (rangeType)
        {
            case eRangeType.SPAWNER:
                {
                    GetComponentInChildren<Spawner>().Enter(collision);
                }
                break;
            case eRangeType.RECEIVER:
                {
                    GetComponentInChildren<Receiver>().Enter(collision);
                }
                break;
            case eRangeType.GIVER:
                {
                    GetComponentInChildren<Giver>().Enter(collision);
                }
                break;
        }

    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        switch (rangeType)
        {
            case eRangeType.SPAWNER:
                {
                    GetComponentInChildren<Spawner>().Interaction(collision);
                }
                break;
            case eRangeType.RECEIVER:
                {
                    GetComponentInChildren<Receiver>().Interaction(collision);
                }
                break;
            case eRangeType.GIVER:
                {
                    GetComponentInChildren<Giver>().Interaction(collision);
                }
                break;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        switch (rangeType)
        {
            case eRangeType.SPAWNER:
                {
                    GetComponentInChildren<Spawner>().Exit();
                }
                break;
            case eRangeType.RECEIVER:
                {
                    GetComponentInChildren<Receiver>().Exit();
                }
                break;
            case eRangeType.GIVER:
                {
                    GetComponentInChildren<Giver>().Exit();
                }
                break;
        }
    }
}
