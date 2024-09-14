using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterData : MonoBehaviour
{
    public int capacity = 10;
    public float sellSpeed = 1.0f;

    // 플레이어와 직원이 위치해야 하는 곳
    public Vector3 staffPlacePosition;
    
    private void Start()
    {
        staffPlacePosition = gameObject.GetComponentInChildren<CheckTouchedWorker>().gameObject.transform.position;
    }
}
