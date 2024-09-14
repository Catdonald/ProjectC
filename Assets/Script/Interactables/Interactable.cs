using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Interactable : MonoBehaviour
{
    protected PlayerController player { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                OnPlayerEnter();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = null;
            OnPlayerExit();
        }
    }

    protected virtual void OnPlayerEnter() { }
    protected virtual void OnPlayerExit() { }
}
