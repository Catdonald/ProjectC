using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool isChairUsing = false;
    private CustomerController sittingCustomer;

    private void OnTriggerExit(Collider other)
    {
        CustomerController customer = other.gameObject.GetComponent<CustomerController>();
        if (customer != null)
        {
            if (sittingCustomer == customer)
            {
                sittingCustomer = null;
                isChairUsing = false;
            }
        }
    }

    public void SetSittingCustomer(CustomerController customer)
    {
        sittingCustomer = customer;
    }

    public CustomerController GetSittingCustomer()
    {
        return sittingCustomer;
    }
}
