using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private bool isSomeoneIn;
    [SerializeField] private MeshRenderer[] entranceMeshes;
    private float curRot = 0;
    private Coroutine doorCoroutine;
    protected override void OnPlayerEnter()
    {
        doorCoroutine = StartCoroutine(OpenDoor());
    }
    protected override void OnPlayerExit()
    {

    }
    protected override void OnCustomerEnter()
    {
        doorCoroutine = StartCoroutine(OpenDoor());
    }
    protected override void OnCustomerExit()
    {

    }

    IEnumerator OpenDoor()
    {
        while (curRot < 90)
        {
            float rotationThisFrame = Time.deltaTime * 50; // 초당 15도 회전
            curRot += rotationThisFrame;

            entranceMeshes[0].transform.Rotate(-Vector3.up, rotationThisFrame);
            entranceMeshes[1].transform.Rotate(Vector3.up, rotationThisFrame);

            yield return null;
        }

        doorCoroutine = null;
    }

    IEnumerator CloseDoor()
    {
        while (curRot > 0)
        {
            float rotationThisFrame = Time.deltaTime * 50; // 초당 15도 회전
            curRot += rotationThisFrame;

            entranceMeshes[0].transform.Rotate(Vector3.up, rotationThisFrame);
            entranceMeshes[1].transform.Rotate(-Vector3.up, rotationThisFrame);

            yield return null;
        }

        doorCoroutine = null;
    }
}
