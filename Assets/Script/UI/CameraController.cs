using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;

    public float moveDuration = 1.0f; 
    public float stayDuration = 1.0f; 

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
        yield return StartCoroutine(MoveToPosition(Camera.main.transform, targetPosition, moveDuration));

        yield return new WaitForSeconds(stayDuration);

        yield return StartCoroutine(MoveToPosition(Camera.main.transform, originalPosition, moveDuration));
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
