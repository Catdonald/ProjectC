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
    [SerializeField] private Transform spawnPoint;
    private Queue<CustomerController> spawnQueue = new Queue<CustomerController>();
    private int maxQueueCount = 10;

    private Material[] materials;

    private Line lineQueue;
    private Receiver receiver;
    private float sellingTimer = 0.0f;
    private float spawnTimer = 0.0f;

    #region Counter Stats
    [SerializeField] private float baseInterval = 1.5f;
    [SerializeField] private int basePrice = 5;
    [SerializeField] private float priceIncrementRate = 1.25f;
    [SerializeField] private int baseStack = 30;
    private float sellingInterval;
    private float spawnInterval;
    private int sellPrice;
    #endregion  

    private void Start()
    {
        receiver = GetComponentInChildren<Receiver>();
        lineQueue = GetComponentInChildren<Line>();

        materials = new Material[4];
        for (int i = 0; i < materials.Length; i++)
        {
            string path = "Materials/CatMat" + (i + 1).ToString();
            materials[i] = Resources.Load<Material>(path);
        }
    }

    private void Update()
    {
        SpawnCustomer();
        SellFoodToCustomer();
    }

    protected override void UpgradeStats()
    {
        sellingInterval = baseInterval / upgradeLevel;
        spawnInterval = (baseInterval * 3) - upgradeLevel;
        receiver.MaxStackCount = baseStack + upgradeLevel * 5;
        sellPrice = Mathf.RoundToInt(priceIncrementRate * basePrice);
        // TODO
        // sellingInterval�� �ٸ� customerSpawner�� spawnInterval�� �پ�� �� �� ������..
        //int profitLevel = GameManager.Instance.GetUpgradeLevel(Upgrade.Profit);
        //sellPrice = Mathf.RoundToInt(Mathf.Pow(priceIncrementRate, profitLevel) * basePrice);
    }

    public int GetStoredFoodCount()
    {
        return receiver.stack.Count;
    }

    public bool IsFoodStorageFull()
    {
        return receiver.IsFull;
    }

    private void SpawnCustomer()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && spawnQueue.Count < maxQueueCount)
        {
            GameObject obj = GameManager.instance.PoolManager.Get(1);
            int randomValue = Random.Range(0, materials.Length);
            obj.GetComponent<SkinnedMeshRenderer>().material = materials[randomValue];
            CustomerController customer = obj.GetComponent<CustomerController>();
            obj.transform.position = spawnPoint.position;
            obj.transform.forward = spawnPoint.forward;
            customer.spawnPoint = spawnPoint;
            customer.line = lineQueue;
            if (StackType == eObjectType.HAMBURGER)
            {
                customer.entrance = GameObject.Find("Entrance_Point1");
                customer.orderInfo = GameManager.instance.GetOrderInfo(0);
            }
            else
            {
                customer.entrance = GameObject.Find("Entrance_Point2");
                customer.orderInfo = GameManager.instance.GetOrderInfo(1);
            }
            spawnQueue.Enqueue(customer);
            spawnTimer = 0.0f;
        }
    }

    private void SellFoodToCustomer()
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

            if (firstCustomer.RemainOrderCount == 0)
            {
                FindAvailableSeat();
            }
        }
    }

    private void FindAvailableSeat()
    {
        var seat = TableManager.Instance.GetAvailableSeat(firstCustomer, stackType);
        if (seat != null)
        {
            spawnQueue.Dequeue();
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
