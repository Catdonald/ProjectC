using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckTouchedWorker : MonoBehaviour
{
    Counter counter;
    Image uiImage;
    
    private bool isTouchedByPlayer = false;
    private bool isTouchedByEmployee = false;

    private float currentSellingGauge = 0.0f;
    private float totalSellingGauge = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        counter = transform.parent.parent.gameObject.GetComponent<Counter>();
        uiImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (counter.customer == null)
        {
            currentSellingGauge = 0.0f;
        }

        if (isTouchedByPlayer || isTouchedByEmployee)
        {
            uiImage.color = Color.green;
            currentSellingGauge += Time.deltaTime;
        }
        else
        {
            uiImage.color = Color.white;
            currentSellingGauge = 0.0f;
        }

        if(currentSellingGauge >= totalSellingGauge)
        {
            // 카운터에서 햄버거 1개 판매
            counter.SellFoodToCustomer();
            currentSellingGauge = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isTouchedByPlayer = true;
        }
        else if (other.gameObject.CompareTag("Employee"))
        {
            isTouchedByEmployee = true;
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
            // 직원 상태 체크
            isTouchedByEmployee = false;
        }
    }
}
