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

            OnPlayerEnter(other);
        }

        if(other.CompareTag("Customer"))
        {
            OnCustomerEnter();
            OnCustomerEnter(other);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerStay(other);
        }

        if (other.CompareTag("Customer"))
        {
            OnCustomerStay(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = null;
            OnPlayerExit();
            OnPlayerExit(other);
        }

        if (other.CompareTag("Customer"))
        {
            OnCustomerExit();
            OnCustomerExit(other);
        }
    }

    protected virtual void OnPlayerEnter() { }
    protected virtual void OnPlayerExit() { }

    protected virtual void OnPlayerEnter(Collider other) { }
    protected virtual void OnPlayerExit(Collider other) { }
    protected virtual void OnPlayerStay(Collider other) { }

    protected virtual void OnCustomerEnter() { }
    protected virtual void OnCustomerExit() { }

    protected virtual void OnCustomerEnter(Collider other) { }
    protected virtual void OnCustomerStay(Collider other) { }
    protected virtual void OnCustomerExit(Collider other) { }
}
