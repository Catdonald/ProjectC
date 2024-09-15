using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Counter : MonoBehaviour
{
    public CounterData counterData;
    public Line lineQueue;
    public CustomerController firstCustomer => lineQueue.Peek();
    public CheckTouchedWorker checkWorker;
    public GameObject casher;

    [SerializeField] private StackType stackType;

    private float sellingTimer = 0.0f;
    private float sellingInterval = 1.5f;
    private Receiver receiver;

    public StackType StackType => stackType;

    private void Start()
    {
        counterData = GetComponent<CounterData>();
        receiver = GetComponentInChildren<Receiver>();
        checkWorker = GetComponentInChildren<CheckTouchedWorker>();
        lineQueue = GetComponentInChildren<Line>();
    }

    private void Update()
    {
        if (lineQueue.QueueCount == 0 || firstCustomer == null || !firstCustomer.HasOrder)
        {
            sellingTimer = 0.0f;
            return;
        }

        if (checkWorker.IsTouchedByPlayer() || checkWorker.IsTouchedByEmployee())
        {
            sellingTimer += Time.deltaTime;
        }
        else
        {
            sellingTimer = 0.0f;
        }

        if (sellingTimer >= sellingInterval)
        {
            sellingTimer = 0.0f;
            SellFoodToCustomer();

            if (firstCustomer.RemainOrderCount == 0)
            {
                FindAvailableSeat();
            }
        }
    }
    public int GetStoredFoodCount()
    {
        return receiver.stack.Count;
    }

    private void SellFoodToCustomer()
    {
        if (firstCustomer.RemainOrderCount > 0 && receiver.stack.Count > 0)
        {
            GameObject obj = receiver.CustomerRequest();
            if (obj != null)
            {
                firstCustomer.ReceiveFood(obj, receiver.objectHeight);
                // TODO
                // CollectMoney();
            }
        }
    }
    
    private void FindAvailableSeat()
    {
        var seat = GameManager.instance.TableManager.GetAvailableSeat(firstCustomer, stackType);
        if (seat != null)
        {
            var customer = lineQueue.RemoveCustomer();
            customer.AssignSeat(seat);
            StartCoroutine(UpdateQueue());
        }
    }

    IEnumerator UpdateQueue()
    {
        yield return new WaitForSeconds(0.5f);
        lineQueue.UpdateCustomerQueue();
    }

    private void CollectMoney(int value)
    {
        // TODO)
    }

}
