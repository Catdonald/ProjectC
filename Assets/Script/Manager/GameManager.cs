using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

/// <summary>
/// 240821 ������
/// ���ӸŴ���, �̱��� 
/// ���� �Ŵ����� ���� �ٸ� �Ŵ��� �ν��Ͻ��� ����
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

    [Header("# Manager")]
    public PoolManager PoolManager;

    [Header("# Upgradables")]
    //[SerializeField] private UpgradableBuyer upgradableBuyer;
    [SerializeField] private List<Upgradable> upgradables = new List<Upgradable>();

    [Header("# UI")]
    //[SerializeField] private TMP_Text moneyText;
    [SerializeField] private OrderInfo burgerOrderInfo;
    //[SerializeField] private OrderInfo burgerPackOrderInfo;

    // Spawner
    public List<Spawner> spawners_burger = new List<Spawner>();
    public List<Spawner> spawners_burgerPack = new List<Spawner>();
    //public List<Spawner> spawners_coffee = new List<Spawner>();

    public List<Counter> counters = new List<Counter>();

    #region Reference Properties
    public OrderInfo BurgerOrderInfo => burgerOrderInfo;
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

    private StoreData data;
    private string storeName;

    void Awake()
    {
        instance = this;
        storeName = SceneManager.GetActiveScene().name;
        data = SaveLoadManager.LoadData<StoreData>(storeName);
        if (data == null)
        {
            data = new StoreData(storeName, 0);
        }
        //TODO
        // money ui

        for (int i = 0; i < data.EmployeeAmount; i++)
        {
            // spawn employee
        }
    }

    private void Start()
    {
        counters = GameObject.FindObjectsOfType<Counter>().ToList();

        var spawnerObjs = GameObject.FindObjectsOfType<Spawner>();
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

        TrashBin = GameObject.FindObjectOfType<Trashbin>();
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
        for(int i = 0; i < value; ++i)
        {
            data.EXP++;
            if (data.EXP >= data.MaxEXP)
            {
                LevelUp();
            }
        }
    }

    public void LevelUp()
    {
        data.Level++;
        data.EXP = 0;
        data.MaxEXP += 3;
        // Level, EXP UI Update
    }

    public int GetEXP()
    { 
        return data.EXP; 
    }

    public int GetMaxEXP()
    {
        return data.MaxEXP; 
    }

    public void AdjustMoney(int value)
    {
        data.Money += value;
        // TODO) money text update
        // update가 아니라 돈을 얻을 때 money text update 시킨다.
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
        AdjustMoney(price);

        switch(upgradeType)
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

    public void SpawnEmployee()
    {

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