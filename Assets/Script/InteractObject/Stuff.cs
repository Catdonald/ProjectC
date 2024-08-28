using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff : MonoBehaviour
{
    [SerializeField] private float followSpeed;

    public void UpdateObjectPosition(Transform followedCube, bool isFollowStart)
    {
        StartCoroutine(FollowingLastObjectPos(followedCube, isFollowStart));
    }

    IEnumerator FollowingLastObjectPos(Transform followedCube, bool isFollowStart)
    {
        while (isFollowStart)
        {
            yield return new WaitForEndOfFrame();
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, followedCube.position.x, followSpeed * Time.deltaTime),
                transform.position.y,
                Mathf.Lerp(transform.position.z, followedCube.position.z, followSpeed * Time.deltaTime));
        }
    }
}