using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private Queue<CustomerController> customerQueue;
    private int queueMaxCount = 10;
    private Transform[] linePositions;
    public int QueueCount { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        customerQueue = new Queue<CustomerController>();
        linePositions = new Transform[queueMaxCount];
        for (int i = 0; i < linePositions.Length; i++)
        {
            linePositions[i] = transform.GetChild(i);
        }
    }

    public bool IsQueueFull()
    {
        return QueueCount >= queueMaxCount;
    }

    public void AddCustomer(CustomerController customer)
    {
        customerQueue.Enqueue(customer);
        AssignQueuePoint(customer, customerQueue.Count - 1);
    }

    public CustomerController RemoveCustomer()
    {
        return customerQueue.Dequeue();
    }

    public void UpdateCustomerQueue()
    {
        int index = 0;
        foreach(var customer in customerQueue)
        {
            AssignQueuePoint(customer, index++);
        }
    }

    public CustomerController Peek()
    {
        if(customerQueue.Count == 0) return null;
        return customerQueue.Peek();
    }

    void AssignQueuePoint(CustomerController customer, int index)
    {
        Vector3 queuePointPos = linePositions[index].position;
        bool isFirst = index == 0;

        customer.UpdateQueue(queuePointPos, isFirst);
    }
}
