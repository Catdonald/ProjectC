using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private bool isSomeoneIn;
    [SerializeField] private MeshRenderer entranceMeshes;
    private float curRot = 0;
    private Coroutine doorCoroutine;

    protected override void OnPlayerEnter(Collider other)
    {
        if (doorCoroutine == null)
        {
            doorCoroutine = StartCoroutine(OpenDoor(other));
        }
    }
    protected override void OnPlayerStay(Collider other)
    {
        if (doorCoroutine == null)
        {
            doorCoroutine = StartCoroutine(OpenDoor(other));
        }
    }
    protected override void OnPlayerExit(Collider other)
    {
        if (doorCoroutine == null)
        {
            doorCoroutine = StartCoroutine(CloseDoor(other));
        }
    }
    protected override void OnCustomerEnter(Collider other)
    {
        if (doorCoroutine == null)
        {
            doorCoroutine = StartCoroutine(OpenDoor(other));
        }
    }
    protected override void OnCustomerStay(Collider other)
    {
        if (doorCoroutine == null)
        {
            doorCoroutine = StartCoroutine(OpenDoor(other));
        }
    }
    protected override void OnCustomerExit(Collider other)
    {
        if (doorCoroutine == null)
        {
            doorCoroutine = StartCoroutine(CloseDoor(other));
        }
    }

    IEnumerator OpenDoor(Collider other)
    {
       // Vector3 forwardDirection = other.transform.GetChild(1).forward; // 플레이어가 바라보는 방향

        while (curRot < 90)
        {
            float rotationThisFrame = Time.deltaTime * 400;
            curRot += rotationThisFrame;

            entranceMeshes.transform.Rotate(other.transform.up, rotationThisFrame);

            yield return null;
        }

        curRot = 90;

        doorCoroutine = null;
    }

    IEnumerator CloseDoor(Collider other)
    {

        while (curRot > 0)
        {
            float rotationThisFrame = Time.deltaTime * 400;
            curRot -= rotationThisFrame;

            entranceMeshes.transform.Rotate(-other.transform.up, rotationThisFrame);

            yield return null;
        }

        curRot = 0;

        doorCoroutine = null;
    }
}
