using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KioskCounter : Upgradable
{
    #region Counter Stats
    [SerializeField] private float baseInterval = 1.0f;
    [SerializeField] private int basePrice_firstFood = 5;
    [SerializeField] private int basePrice_secondFood = 5;
    [SerializeField] private float priceIncrementRate = 1.25f;
    [SerializeField] private int baseStack = 30;
    #endregion  
    [SerializeField] private Transform customerSpawnPoint;
    [SerializeField] private Transform[] KioskPoint;
    [SerializeField] private Receiver burgerReceiver;
    [SerializeField] private Receiver subMenuReceiver;

    private KioskCustomer[] kioskCustomers;

    private float sellingTimer = 0.0f;
    private float spawnTimer = 0.0f;
    private float sellingInterval;
    private float spawnInterval;
    private int sellPrice_firstFood;
    private int sellPrice_secondFood;
    private bool isFirstMenuSelling = false;
    private bool isSecondMenuSelling = false;
    private const int maxQueueCount = 2;
    private Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        kioskCustomers = new KioskCustomer[maxQueueCount];
        materials = new Material[4];
        for (int i = 0; i < materials.Length; i++)
        {
            string path = "Materials/CatMat" + (i + 1).ToString();
            materials[i] = Resources.Load<Material>(path);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnCustomer();
        SellFoodToCustomer();
    }

    public override void UpgradeStats()
    {
        sellingInterval = baseInterval / upgradeLevel;
        spawnInterval = (baseInterval * 3) - upgradeLevel;
        burgerReceiver.MaxStackCount = baseStack + upgradeLevel * 5;
        subMenuReceiver.MaxStackCount = baseStack + upgradeLevel * 5;
        int profitLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.Profit);
        sellPrice_firstFood = Mathf.RoundToInt(Mathf.Pow(priceIncrementRate, profitLevel) * basePrice_firstFood);
        sellPrice_secondFood = Mathf.RoundToInt(Mathf.Pow(priceIncrementRate, profitLevel) * basePrice_secondFood);
    }

    private void SpawnCustomer()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && HasAvailableKiosk())
        {
            spawnTimer = 0.0f;
            GameObject obj = GameManager.instance.PoolManager.SpawnObject("Customer_kiosk");
            int randomValue = Random.Range(0, materials.Length);
            obj.GetComponent<SkinnedMeshRenderer>().material = materials[randomValue];
            obj.transform.position = customerSpawnPoint.position;
            obj.transform.forward = customerSpawnPoint.forward;
            KioskCustomer customer = obj.GetComponent<KioskCustomer>();
            int index = GetAvailableKioskIndex();           
            kioskCustomers[index] = customer;
            customer.Init(customerSpawnPoint.transform, KioskPoint[index].gameObject, index);
        }
    }

    private void SellFoodToCustomer()
    {
        if (AreAllKiosksEmpty())
        {
            sellingTimer = 0.0f;
            return;
        }

        sellingTimer += Time.deltaTime;
        if (sellingTimer >= sellingInterval)
        {
            sellingTimer = 0.0f;
            if (!isFirstMenuSelling)
            {
                StartCoroutine(SellFirstFood());
            }
            if (!isSecondMenuSelling)
            {
                StartCoroutine(SellSecondFood());
            }

            for(int i = 0; i < kioskCustomers.Length; i++)
            {
                if (kioskCustomers[i] == null) continue;
                if (kioskCustomers[i].FirstOrderDone && kioskCustomers[i].SecondOrderDone)
                {
                    kioskCustomers[i].FinishOrder();
                    kioskCustomers[i] = null;
                }
            }
        }
    }

    IEnumerator SellFirstFood()
    {
        isFirstMenuSelling = true;
        int index = 0;
        while (burgerReceiver.Count > 0)
        {
            // has no customers in all kiosks
            if (AreAllKiosksEmpty())
            {
                isFirstMenuSelling = false;
                yield break;
            }
            // kiosk 한대씩 번갈아 판매
            var customer = kioskCustomers[index];
            if (customer != null)
            {
                if (customer.HasOrder && customer.FirstOrderCount > 0)
                {
                    var food = burgerReceiver.RequestObject();
                    customer.ReceiveFirstFood(food);
                    // TODO
                    // CollectMoney();
                }
                index++;
                index %= 2;
            }
            yield return new WaitForSeconds(0.2f);
        }
        isFirstMenuSelling = false;
    }

    IEnumerator SellSecondFood()
    {
        isSecondMenuSelling = true;
        int index = 0;
        while (subMenuReceiver.Count > 0)
        {
            // has no customers in all kiosks
            if (AreAllKiosksEmpty())
            {
                isSecondMenuSelling = false;
                yield break;
            }
            // kiosk 한대씩 번갈아 판매
            var customer = kioskCustomers[index];
            if (customer != null)
            {
                if (customer.HasOrder && customer.SecondOrderCount > 0)
                {
                    var food = subMenuReceiver.RequestObject();
                    customer.ReceiveSecondFood(food);
                    // TODO
                    // CollectMoney();
                }
                index++;
                index %= 2;
            }
            yield return new WaitForSeconds(0.2f);
        }
        isSecondMenuSelling = false;
    }

    private bool HasAvailableKiosk()
    {
        return GetAvailableKioskIndex() != -1;
    }

    private int GetAvailableKioskIndex()
    {
        for (int i = 0; i < kioskCustomers.Length; i++)
        {
            if (kioskCustomers[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    private bool AreAllKiosksEmpty()
    {
        for (int i = 0; i < kioskCustomers.Length; i++)
        {
            if (kioskCustomers[i] != null)
            {
                return false;
            }
        }
        return true;
    }
}
