using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CameraController : MonoBehaviour
{
    public bool IsMoving { get; set; } = false;
    public float waitDuration = 1.5f;

    private float moveDuration = 0.3f; 
    private float stayDuration = 2.0f;
    private Vector3 originalPosition; 

    public void ShowPosition(Vector3 targetPos)
    {        
        if(IsMoving)
            return;
        originalPosition = Camera.main.transform.position;
        StartCoroutine(MoveCamera(targetPos));
    }

    IEnumerator MoveCamera(Vector3 position)
    {
        IsMoving = true;
        Vector3 cameraLocalPos = Camera.main.transform.localPosition;
        Vector3 targetPos = new Vector3(position.x + cameraLocalPos.x, cameraLocalPos.y, position.z + cameraLocalPos.z);

        moveDuration = Vector3.Distance(cameraLocalPos, targetPos) / 10;

        yield return new WaitForSeconds(waitDuration);
        Camera.main.transform.DOMove(targetPos, moveDuration).SetEase(Ease.Linear);

        yield return new WaitForSeconds(stayDuration);
        Camera.main.transform.DOMove(originalPosition, moveDuration).SetEase(Ease.Linear);

        IsMoving = false;
    }
}
