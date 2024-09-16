using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : Interactable
{
    [SerializeField] private GameObject linkedUI;

    protected override void OnPlayerEnter()
    {
        linkedUI.SetActive(true);
    }
    protected override void OnPlayerExit()
    {
        linkedUI.SetActive(false);
    }
}
