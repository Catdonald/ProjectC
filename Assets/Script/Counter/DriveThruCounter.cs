using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveThruCounter : WorkStation
{
    public GameObject casher;
    public Receiver Receiver => receiver;
    public eObjectType StackType => receiver.type;
    public CarController firstCar => cars.Peek();
    public int GetStoredFoodCount => receiver.stack.Count;
    public bool IsStorageFull => receiver.IsFull;

    #region Counter Stats
    [SerializeField] private float baseInterval = 1.5f;
    [SerializeField] private int basePrice = 20;
    [SerializeField] private float priceIncrementRate = 1.25f;
    [SerializeField] private int baseStack = 30;
    #endregion
    [SerializeField] private Receiver receiver;
    [SerializeField] private Transform carSpawnPoint;
    [SerializeField] private Transform carDespawnPoint;
    [SerializeField] private CarController[] carPrefabs;

    private Queue<CarController> cars = new Queue<CarController>();
    private Line line;
    private MoneyPile moneyPile;
    private float sellingTimer = 0.0f;
    private float spawnTimer = 0.0f;
    private float sellingInterval;
    private float spawnInterval;
    private int sellPrice;
    private bool isFinishServing = false;
    const int maxQueueCount = 10;

    void Start()
    {
        line = GetComponentInChildren<Line>();
        moneyPile = GetComponentInChildren<MoneyPile>();
    }

    void Update()
    {
        SpawnCar();
        SellPackage();
    }

    public override void UpgradeStats()
    {
        sellingInterval = baseInterval / upgradeLevel;
        spawnInterval = (baseInterval * 3) - upgradeLevel;
        receiver.MaxStackCount = baseStack + upgradeLevel * 5;
        sellPrice = Mathf.RoundToInt(priceIncrementRate * basePrice);
        int profitLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.Profit);
        sellPrice = Mathf.RoundToInt(Mathf.Pow(priceIncrementRate, profitLevel) * basePrice);
    }

    private void SpawnCar()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && cars.Count < maxQueueCount)
        {
            spawnTimer = 0.0f;
            int randomNum = Random.Range(0, carPrefabs.Length);
            CarController car = Instantiate(carPrefabs[randomNum], carSpawnPoint.position, carSpawnPoint.rotation);
            cars.Enqueue(car);
            car.Init(line, carDespawnPoint.position, cars.Count - 1);
        }
    }

    private void SellPackage()
    {
        if (cars.Count == 0 || !firstCar.HasOrder)
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
                if (firstCar.OrderCount > 0 && receiver.Count > 0)
                {
                    GameObject obj = receiver.RequestObject();
                    if (obj != null)
                    {
                        firstCar.ReceiveFood(obj.transform);
                        CollectMoney();
                    }
                }
            }
            if (firstCar.OrderCount == 0 && !isFinishServing)
            {
                StartCoroutine(FinishServing());
            }
        }
    }

    IEnumerator FinishServing()
    {
        isFinishServing = true;
        yield return new WaitForSeconds(0.5f);

        var car = cars.Dequeue();
        car.Exit();

        foreach (var eachCar in cars)
        {
            eachCar.UpdateQueue();
        }

        sellingTimer = 0.0f;
        isFinishServing = true;
    }

    private void CollectMoney()
    {
        for(int i = 0; i < sellPrice; i++)
        {
            moneyPile.AddMoney();
        }
    }
}
