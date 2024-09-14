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
    // 콜라이더 영역에 Staff가 있을 때
    private bool isTouchedByStaff = false;

    // Start is called before the first frame update
    void Start()
    {
        counter = transform.parent.parent.gameObject.GetComponent<Counter>();
        uiImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouchedByPlayer || isTouchedByStaff)
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
        else if (other.gameObject.CompareTag("Staff"))
        {
            isTouchedByStaff = true;
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
        else if (other.gameObject.CompareTag("Staff"))
        {
            isTouchedByStaff = false;
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
    public bool IsTouchedByStaff()
    {
        return isTouchedByStaff;
    }
}
