using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public CustomerController customer;
    private int storagedFoodCount;
    public int StoragedFoodCount
    {
        get { return storagedFoodCount; }
        set
        {
            if (value < 0)
                storagedFoodCount = 0;
            else
                storagedFoodCount = value;
        }
    }

    private void Start()
    {
        StoragedFoodCount = 100;
    }

    public void SellFoodToCustomer()
    {
        if (customer == null)
            return;
        if (customer.OrderCount > 0)
        {
            if (StoragedFoodCount > 0)
            {
                StoragedFoodCount--;
                customer.ReceiveFood(1);
                Debug.Log("Counter : " + StoragedFoodCount);
                // 카운터에 쌓인 햄버거 한개 손님한테 날아감
            }
        }
    }
}
