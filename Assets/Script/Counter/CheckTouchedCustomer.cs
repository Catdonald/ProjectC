using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTouchedCustomer : MonoBehaviour
{
    Counter counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = transform.parent.gameObject.GetComponent<Counter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Customer"))
        {
            counter.customer = other.gameObject.GetComponent<CustomerController>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            counter.customer = null;
        }
    }
}
