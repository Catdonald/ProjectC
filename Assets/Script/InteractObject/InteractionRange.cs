using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRange : MonoBehaviour
{
    public Receiver stack;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        GetComponentInChildren<Receiver>().Enter(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        GetComponentInChildren<Receiver>().Interaction(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        GetComponentInChildren<Receiver>().Exit();
    }
}
