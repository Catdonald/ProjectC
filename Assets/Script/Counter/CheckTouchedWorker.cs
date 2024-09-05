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

    private float sellingGauge = 0.0f;

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
            sellingGauge = 0.0f;
        }

        if (isTouchedByPlayer || isTouchedByEmployee)
        {
            uiImage.color = Color.green;
            sellingGauge += Time.deltaTime * 0.75f;
        }
        else
        {
            uiImage.color = Color.white;
            sellingGauge = 0.0f;
        }

        if(sellingGauge >= 1.0f)
        {
            // 카운터에서 햄버거 1개 판매
            counter.SellFoodToCustomer();
            sellingGauge = 0.0f;
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
            isTouchedByEmployee = false;
        }
    }
}
