using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
#endif

public class Counter : WorkStation
{
    public GameObject casher;
    public Receiver Receiver => receiver;
    public eObjectType StackType => receiver.type;
    public CustomerController firstCustomer => lineQueue.Peek();
    public int GetStoredFoodCount => receiver.stack.Count;
    public bool IsStorageFull => receiver.IsFull;

    #region Counter Stats
    [SerializeField] private float baseInterval = 1.5f;
    [SerializeField] private int basePrice = 5;
    [SerializeField] private float priceIncrementRate = 1.25f;
    [SerializeField] private int baseStack = 30;
    #endregion  
    [SerializeField] private Transform customerSpawnPoint;
    [SerializeField] private Transform entrancePoint;
    [SerializeField] private Receiver receiver;

    private Queue<CustomerController> spawnQueue = new Queue<CustomerController>();
    private Queue<CustomerController> lineQueue = new Queue<CustomerController>();
    private Line line;
    private MoneyPile moneyPile;
    private Material[] materials;
    private float sellingTimer = 0.0f;
    private float spawnTimer = 0.0f;
    private float sellingInterval;
    private float spawnInterval;
    private int sellPrice;
    const int maxQueueCount = 10;

    private void Start()
    {
        line = GetComponentInChildren<Line>();
        moneyPile = GetComponentInChildren<MoneyPile>();
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

    public override void UpgradeStats()
    {
        sellingInterval = baseInterval / upgradeLevel;
        spawnInterval = (baseInterval * 3) - upgradeLevel;
        receiver.MaxStackCount = baseStack + upgradeLevel * 5;
        int profitLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.Profit);
        sellPrice = Mathf.RoundToInt(Mathf.Pow(priceIncrementRate, profitLevel) * basePrice);
    }

    public void AddCustomer(CustomerController customer)
    {
        lineQueue.Enqueue(customer);
        AssignQueuePoint(customer, lineQueue.Count - 1);
    }

    private void SpawnCustomer()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && spawnQueue.Count < maxQueueCount)
        {
            spawnTimer = 0.0f;
            GameObject obj = GameManager.instance.PoolManager.SpawnObject("Customer");
            int randomValue = Random.Range(0, materials.Length);
            obj.GetComponent<SkinnedMeshRenderer>().material = materials[randomValue];
            obj.transform.position = customerSpawnPoint.position;
            obj.transform.forward = customerSpawnPoint.forward;
            CustomerController customer = obj.GetComponent<CustomerController>();
            OrderInfo orderInfo;
            if (StackType == eObjectType.HAMBURGER)
            {
                orderInfo = GameManager.instance.GetOrderInfo(0);
            }
            else
            {
                orderInfo = GameManager.instance.GetOrderInfo(1);
            }
            customer.Init(customerSpawnPoint, entrancePoint, this, orderInfo);
            spawnQueue.Enqueue(customer);
        }
    }

    private void SellFoodToCustomer()
    {
        if (lineQueue.Count == 0 || firstCustomer == null || !firstCustomer.HasOrder)
        {
            sellingTimer = 0.0f;
            return;
        }

        sellingTimer += Time.deltaTime;

        if (sellingTimer >= sellingInterval)
        {
            sellingTimer = 0.0f;
            if (HasWorker)
            {
                if (firstCustomer.OrderCount > 0 && receiver.Count > 0)
                {
                    GameObject obj = receiver.RequestObject();
                    if (obj != null)
                    {
                        firstCustomer.ReceiveFood(obj, receiver.type, GameManager.instance.GetStackOffset(receiver.type));
                        CollectMoney();
                    }
                }
            }

            if (firstCustomer.OrderCount == 0)
            {
                FindAvailableSeat();
            }
        }
    }

    private void FindAvailableSeat()
    {
        var seat = TableManager.Instance.GetAvailableSeat(firstCustomer, StackType);
        if (seat != null)
        {
            spawnQueue.Dequeue();
            var customer = lineQueue.Dequeue();
            customer.AssignSeat(seat);
            StartCoroutine(UpdateQueue());
        }
    }

    IEnumerator UpdateQueue()
    {
        yield return new WaitForSeconds(0.5f);
        int index = 0;
        foreach (var customer in lineQueue)
        {
            AssignQueuePoint(customer, index++);
        }
    }

    private void AssignQueuePoint(CustomerController customer, int index)
    {
        Vector3 queuePointPos = line.GetLinePosition(index);
        bool isFirst = index == 0;

        customer.UpdateQueue(queuePointPos, isFirst);
    }

    private void CollectMoney()
    {
        for (int i = 0; i < sellPrice; i++)
        {
            moneyPile.AddMoney();
        }
    }

}
