using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool IsMoving { get; private set; } = false;

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

        StartCoroutine(MoveCamera(pos));
    }

    IEnumerator MoveCamera(Vector3 targetPosition)
    {
        IsMoving = true;
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(MoveToPosition(Camera.main.transform, targetPosition, moveDuration));

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
