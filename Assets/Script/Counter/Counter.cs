using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public CustomerController customer;
    private Receiver receiver;

    private void Start()
    {
        receiver = GetComponentInChildren<Receiver>();
    }

    public void SellFoodToCustomer()
    {
        if (customer == null)
            return;
        if (customer.OrderCount > 0)
        {
            if(receiver.stack.Count > 0)
            {
                GameObject obj = receiver.CustomerRequest();
                if (obj != null)
                {
                    customer.ReceiveFood(obj, receiver.objectHeight);
                }
            }
        }
    }
}
