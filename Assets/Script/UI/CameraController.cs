using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool IsMoving { get; set; } = false;
    public float waitDuration = 1.5f;

    private float moveDuration = 0.2f; 
    private float stayDuration = 2.0f;
    
    private Camera mainCamera;
    private Vector3 originalPosition; 

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }
    public void ShowPosition(Vector3 pos)
    {
        originalPosition = Camera.main.transform.position;
        Vector3 targetPos = Camera.main.transform.localPosition;
        targetPos.x += pos.x;
        targetPos.z += pos.z;
        StartCoroutine(MoveCamera(targetPos));
    }

    IEnumerator MoveCamera(Vector3 targetPosition)
    {
        IsMoving = true;
        yield return new WaitForSeconds(waitDuration);
        yield return StartCoroutine(MoveToPosition(Camera.main.transform, targetPosition, moveDuration));

        yield return new WaitForSeconds(stayDuration);

        yield return StartCoroutine(MoveToPosition(Camera.main.transform, originalPosition, moveDuration));
        IsMoving = false;
        waitDuration = 1.5f;
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
