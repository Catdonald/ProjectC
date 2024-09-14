using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterData : MonoBehaviour
{
    public int capacity = 10;
    public float sellSpeed = 1.0f;

    // �÷��̾�� ������ ��ġ�ؾ� �ϴ� ��
    public Vector3 staffPlacePosition;
    
    private void Start()
    {
        staffPlacePosition = gameObject.GetComponentInChildren<CheckTouchedWorker>().gameObject.transform.position;
    }
}
