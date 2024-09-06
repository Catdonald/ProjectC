using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private Queue<GameObject> customerQueue;
    private int queueMaxCount = 10;
    private GameObject[] linePositions;
    public int currentQueueCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        customerQueue = new Queue<GameObject>();
        linePositions = new GameObject[queueMaxCount];
        for (int i = 0; i < linePositions.Length; i++)
        {
            linePositions[i] = GameObject.Find("LinePosition" + (i + 1).ToString());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            if (customerQueue.Count < queueMaxCount)
            {
                CustomerController customer = other.gameObject.GetComponent<CustomerController>();
                if (customer.GetCurrentState() == CustomerController.State.LINEMOVE)
                {
                    customerQueue.Enqueue(other.gameObject);
                    customer.SetAgentDestination(linePositions[customerQueue.Count - 1].transform.position);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            CustomerController customer = other.gameObject.GetComponent<CustomerController>();
            if (customer.GetCurrentState() == CustomerController.State.MOVETOTABLE)
            {
                customerQueue.Dequeue();
                currentQueueCount--;
                int index = 0;
                foreach (GameObject eachObj in customerQueue)
                {
                    eachObj.GetComponent<CustomerController>().SetAgentDestination(linePositions[index++].transform.position);
                }
            }
        }
    }

    public bool IsLineQueueFull()
    {
        return currentQueueCount >= queueMaxCount;
    }
}
