using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using DG.Tweening;
using System.Text;

/// <summary>
/// Game Manager
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Base Setting")]
    [SerializeField] private int baseUpgradePrice = 250;
    [SerializeField, Range(1.01f, 2.0f)] private float upgradeGrowthFactor = 1.5f;
    [SerializeField] private int baseUnlockPrice = 75;
    [SerializeField, Range(1.01f, 2.0f)] private float unlockGrowthFactor = 1.2f;
    [SerializeField] private long startingMoney = 1000;
    [SerializeField] private int startingMaxExp = 10;

    [Header("# Manager")]
    public PoolManager PoolManager;

    [Header("# Employee")]
    [SerializeField] private Transform employeeSpawner;
    [SerializeField] private GameObject employeePrefab;
    [SerializeField, Range(2.0f, 5.0f)] private float employeeSpawnRadius = 3.0f;

    [Header("# Upgradables")]
    //[SerializeField] private UpgradeBox upgradeBox;
    private int upgradeIndex = -1;
    [SerializeField] private GameObject upgradeButton;
    public List<Upgradable> upgradables = new List<Upgradable>();
    [SerializeField] public Upgradable currentUpgradableObj;

    [Header("# UI")]
    //[SerializeField] private TMP_Text moneyText;
    [SerializeField] private OrderInfo[] orderInfo; // 0: burger, 1: sub-menu
    //[SerializeField] private OrderInfo burgerPackOrderInfo;

    // Spawner
    public List<Spawner> spawners_burger = new List<Spawner>();
    public List<Spawner> spawners_burgerPack = new List<Spawner>();
    //public List<Spawner> spawners_coffee = new List<Spawner>();

    public List<Counter> counters = new List<Counter>();

    #region Reference Properties 
    public DriveThruCounter DriveThruCounter { get; private set; }
    public Trashbin TrashBin { get; private set; }
    public int PaidAmount
    {
        get => data.PaidAmount;
        set => data.PaidAmount = value;
    }
    public int UnlockCount
    {
        get => data.UnlockCount;
        set => data.UnlockCount = value;
    }
    #endregion

    public event System.Action OnUpgrade;
    public event System.Action<float> OnUnlock;

    public StoreData data;
    private string storeName;

    void Awake()
    {
        instance = this;
        storeName = "store name";
        data = SaveLoadManager.LoadData<StoreData>(storeName);
        if (data == null)
        {
            data = new StoreData(storeName, startingMoney, startingMaxExp, 1);
        }
        AdjustMoney(0);

        for (int i = 0; i < data.EmployeeAmount; i++)
        {
            SpawnEmployee();
        }
    }
    private void Start()
    {
        SetNowUpgradableObject();

        counters = GameObject.FindObjectsOfType<Counter>(true).ToList();

        var spawnerObjs = GameObject.FindObjectsOfType<Spawner>(true);
        foreach (Spawner spawner in spawnerObjs)
        {
            if (spawner.type == eObjectType.HAMBURGER)
            {
                spawners_burger.Add(spawner);
            }
            else if (spawner.type == eObjectType.BURGERPACK)
            {
                spawners_burgerPack.Add(spawner);
            }
        }

        DriveThruCounter = GameObject.FindObjectOfType<DriveThruCounter>(true);
        TrashBin = GameObject.FindObjectOfType<Trashbin>();
    }
    public void SetNowUpgradableObject()
    {
        upgradeIndex++;
        if (upgradables.Count < upgradeIndex)
            upgradeIndex = 0;

        currentUpgradableObj = upgradables[upgradeIndex];

        Vector3 pos = currentUpgradableObj.BuyingPosition;
        pos.y = 0.18f;

        upgradeButton.transform.position = pos;
    }

    void LoadDataFromCSV<T>(string filename, List<T> dataLst, Func<string[], T> parser)
    {
        TextAsset csvData = Resources.Load<TextAsset>(filename);
        if (csvData == null)
        {
            Debug.LogError("CSV is not founded: " + filename);
            return;
        }

        StringReader reader = new StringReader(csvData.text);

        bool isFirstLine = true;
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            string[] values = line.Split(',');

            T data = parser(values);
            dataLst.Add(data);
        }
    }


    public int GetLevel()
    {
        return data.Level;
    }

    public void AddEXP(int value)
    {
        for (int i = 0; i < value; ++i)
        {
            data.EXP++;
            if (data.EXP >= data.NextEXP)
            {
                LevelUp();
            }
        }
    }

    public void LevelUp()
    {
        data.Level++;
        data.EXP = 0;
        data.NextEXP += 3;
    }

    public int GetEXP()
    {
        return data.EXP;
    }

    public int GetMaxEXP()
    {
        return data.NextEXP;
    }

    public void AdjustMoney(int value)
    {
        data.Money += value;
    }

    public long GetMoney()
    {
        return data.Money;
    }

    public string GetFormattedMoney(long money)
    {
        if (money < 1000) return money.ToString();
        else if (money < 1000000) return (money / 1000f).ToString("0.##") + "k";
        else if (money < 1000000000) return (money / 1000000f).ToString("0.##") + "m";
        else if (money < 1000000000000) return (money / 1000000000f).ToString("0.##") + "b";
        else return (money / 1000000000000f).ToString("0.##") + "t";
    }

    public void PurchaseUpgrade(UpgradeType upgradeType)
    {
        int price = GetUpgradePrice(upgradeType);
        AdjustMoney(-price);

        switch (upgradeType)
        {
            case UpgradeType.EmployeeSpeed:
                data.EmployeeSpeed++;
                break;
            case UpgradeType.EmployeeCapacity:
                data.EmployeeCapacity++;
                break;
            case UpgradeType.EmployeeAmount:
                data.EmployeeAmount++;
                SpawnEmployee();
                break;
            case UpgradeType.PlayerSpeed:
                data.PlayerSpeed++;
                break;
            case UpgradeType.PlayerCapacity:
                data.PlayerCapacity++;
                break;
            case UpgradeType.Profit:
                data.Profit++;
                break;
            default:
                break;
        }
        SaveLoadManager.SaveData<StoreData>(data, storeName);
        OnUpgrade?.Invoke();
    }

    public int GetUpgradePrice(UpgradeType upgradeType)
    {
        int currentLevel = GetUpgradeLevel(upgradeType);
        return Mathf.RoundToInt(Mathf.Round(baseUpgradePrice * Mathf.Pow(upgradeGrowthFactor, currentLevel)) / 50.0f) * 50;
    }

    public int GetUpgradeLevel(UpgradeType upgradeType)
    {
        int level = 0;
        switch (upgradeType)
        {
            case UpgradeType.EmployeeSpeed:
                level = data.EmployeeSpeed;
                break;
            case UpgradeType.EmployeeCapacity:
                level = data.EmployeeCapacity;
                break;
            case UpgradeType.EmployeeAmount:
                level = data.EmployeeAmount;
                break;
            case UpgradeType.PlayerSpeed:
                level = data.PlayerSpeed;
                break;
            case UpgradeType.PlayerCapacity:
                level = data.PlayerCapacity;
                break;
            case UpgradeType.Profit:
                level = data.Profit;
                break;
            default:
                break;
        }
        return level;
    }

    public OrderInfo GetOrderInfo(int index)
    {
        return orderInfo[index];
    }

    public void SpawnEmployee()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * employeeSpawnRadius;
        Vector3 randomPos = employeeSpawner.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        Instantiate(employeePrefab, randomPos, employeeSpawner.rotation);
    }
}

public enum UpgradeType
{
    EmployeeSpeed,
    EmployeeCapacity,
    EmployeeAmount,
    PlayerSpeed,
    PlayerCapacity,
    Profit
}