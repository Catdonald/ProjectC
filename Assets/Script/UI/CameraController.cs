using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool IsMoving { get; set; } = false;
    public float waitDuration = 1.5f;

    private float moveDuration = 0.2f; 
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
        yield return new WaitForSeconds(waitDuration);
        yield return StartCoroutine(MoveToPosition(Camera.main.transform, targetPos, moveDuration));

        yield return new WaitForSeconds(stayDuration);
        yield return StartCoroutine(MoveToPosition(Camera.main.transform, originalPosition, moveDuration));
        
        IsMoving = false;
    }

    IEnumerator MoveToPosition(Transform transform, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
