using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkingSpot : MonoBehaviour
{
    private EmployeeController workingEmployee;
    private Image uiImage;

    private bool isPlayerInSpot = false;
    // 콜라이더 영역에 Employee가 있을 때
    private bool isEmployeeInSpot = false;

    public EmployeeController WorkingEmployee => workingEmployee;
    public bool HasWorker => isPlayerInSpot || isEmployeeInSpot;

    // Start is called before the first frame update
    void Start()
    {
        uiImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInSpot || isEmployeeInSpot)
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
            isPlayerInSpot = true;
        }
        else if (other.gameObject.CompareTag("Employee"))
        {
            isEmployeeInSpot = true;
            if (workingEmployee == null)
            {
                workingEmployee = other.gameObject.GetComponent<EmployeeController>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInSpot = false;
        }
        else if (other.gameObject.CompareTag("Employee"))
        {
            isEmployeeInSpot = false;
            if (workingEmployee == other.gameObject.GetComponent<EmployeeController>())
            {
                workingEmployee = null;
            }
        }
    }
}
