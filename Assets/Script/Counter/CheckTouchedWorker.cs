using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckTouchedWorker : MonoBehaviour
{
    public EmployeeController touchedEmployee;
    Counter counter;
    Image uiImage;

    private bool isTouchedByPlayer = false;
    // �ݶ��̴� ������ Staff�� ���� ��
    private bool isTouchedByEmployee = false;

    // Start is called before the first frame update
    void Start()
    {
        counter = transform.parent.parent.gameObject.GetComponent<Counter>();
        uiImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouchedByPlayer || isTouchedByEmployee)
        {
            uiImage.color = Color.green;
        }
        else
        {
            uiImage.color = Color.white;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchedByPlayer = true;
        }
        else if (other.gameObject.CompareTag("Employee"))
        {
            isTouchedByEmployee = true;
            if(touchedEmployee == null)
            {
                touchedEmployee = other.gameObject.GetComponent<EmployeeController>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchedByPlayer = false;
        }
        else if (other.gameObject.CompareTag("Employee"))
        {
            isTouchedByEmployee = false;
            if(touchedEmployee == other.gameObject.GetComponent<EmployeeController>())
            {
                touchedEmployee = null;
            }
        }
    }

    public bool IsTouchedByPlayer()
    {
        return isTouchedByPlayer;
    }
    public bool IsTouchedByEmployee()
    {
        return isTouchedByEmployee;
    }
}
