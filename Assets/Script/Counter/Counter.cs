using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Counter : WorkStation
{
    public GameObject casher;
    public eObjectType StackType => stackType;
    public CustomerController firstCustomer => lineQueue.Peek();

    [SerializeField] private eObjectType stackType;
   
    private Line lineQueue;
    private Receiver receiver;
    private float sellingTimer = 0.0f;

    #region Counter Stats
    [SerializeField] private float baseSellingInterval = 1.5f;
    [SerializeField] private int basePrice = 5;
    [SerializeField] private float priceIncrementRate = 1.25f;
    [SerializeField] private int baseStack = 30;
    private float sellingInterval;
    private int sellPrice;
    #endregion  

    private void Start()
    {
        receiver = GetComponentInChildren<Receiver>();
        lineQueue = GetComponentInChildren<Line>();
    }

    private void Update()
    {
        if (lineQueue.QueueCount == 0 || firstCustomer == null || !firstCustomer.HasOrder)
        {
            sellingTimer = 0.0f;
            return;
        }

        if (HasWorker)
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

    protected override void UpgradeStats()
    {
        sellingInterval = baseSellingInterval / upgradeLevel;
        receiver.MaxStackCount = baseStack + upgradeLevel * 5;
        sellPrice = Mathf.RoundToInt(priceIncrementRate * basePrice);
        // TODO
        // sellingInterval�� �ٸ� customerSpawner�� spawnInterval�� �پ�� �� �� ������..
        //int profitLevel = GameManager.Instance.GetUpgradeLevel(Upgrade.Profit);
        //sellPrice = Mathf.RoundToInt(Mathf.Pow(priceIncrementRate, profitLevel) * basePrice);
    }

    void OnCollisionEnter(Collision collision)
    {

    }

    public int GetStoredFoodCount()
    {
        return receiver.stack.Count;
    }

    public bool IsFoodStorageFull()
    {
        return receiver.IsFull;
    }

    private void SellFoodToCustomer()
    {
        if (firstCustomer.RemainOrderCount > 0 && receiver.stack.Count > 0)
        {
            GameObject obj = receiver.RequestObject();
            if (obj != null)
            {
                firstCustomer.ReceiveFood(obj, receiver.type, receiver.objectHeight);
                // TODO
                // CollectMoney();
            }
        }
    }
    
    private void FindAvailableSeat()
    {
        var seat = TableManager.Instance.GetAvailableSeat(firstCustomer, stackType);
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
