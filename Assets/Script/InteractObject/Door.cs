using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private float openDuration = 0.4f;
    [SerializeField] private float closeDuration = 0.5f;
    private Vector3 openAngle = new Vector3(0, 90, 0);
    private bool isOpen = false;

    protected override void OnPlayerEnter()
    {
        OpenDoor(player.transform);
    }

    protected override void OnPlayerExit()
    {
        CloseDoor();
    }

    public void OpenDoor(Transform interactor)
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;
        Vector3 direction = (interactor.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, direction);
        Vector3 targetAngle = openAngle * Mathf.Sign(dotProduct);
        doorTransform.DOLocalRotate(targetAngle, openDuration, RotateMode.LocalAxisAdd);
    }

    public void CloseDoor()
    {
        if(player != null)
        {
            return;
        }

        isOpen = false;
        doorTransform.DOLocalRotate(Vector3.zero, closeDuration).SetEase(Ease.OutBounce);
    }
}
