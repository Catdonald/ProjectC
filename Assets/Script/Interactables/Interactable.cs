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

        if(other.CompareTag("Customer"))
        {
            OnCustomerEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = null;
            OnPlayerExit();
        }

        if (other.CompareTag("Customer"))
        {
            OnCustomerExit();
        }
    }

    protected virtual void OnPlayerEnter() { }
    protected virtual void OnPlayerExit() { }

    protected virtual void OnCustomerEnter() { }
    protected virtual void OnCustomerExit() { }
}
