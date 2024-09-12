using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerInteraction : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        GetComponentInChildren<Spawner>().Enter(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        GetComponentInChildren<Spawner>().Interaction(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        GetComponentInChildren<Spawner>().Exit();
    }
}
