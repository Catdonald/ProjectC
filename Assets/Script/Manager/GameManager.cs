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
    [SerializeField] private long startingMoney = 10000;

    [Header("# Manager")]
    public PoolManager PoolManager;
    public SoundManager SoundManager;

    [Header("# Employee")]
    [SerializeField] private Transform employeeSpawner;
    [SerializeField] private GameObject employeePrefab;
    [SerializeField, Range(2.0f, 5.0f)] private float employeeSpawnRadius = 3.0f;

    [Header("# Upgradables")]
    [SerializeField] private UpgradeBox upgradeButton;
    public List<Upgradable> upgradables = new List<Upgradable>();

    [Header("# Effects")]
    [SerializeField] private ParticleSystem upgradeParticle;

    [Header("# UI")]
    [SerializeField] private OfflineReward offlineReward;
    [SerializeField] private DisplayProgress displayProgress;
    [SerializeField] private Text displayMoneyText;
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private OrderInfo[] orderInfo; // 0: burger, 1: sub-menu, 2: driveThru
    [SerializeField] private KioskOrderInfo[] kioskOrderInfo;

    [Header("# Offset")]
    [SerializeField] private float burgerOffset = 0.4f;
    [SerializeField] private float burgerPackOffset = 0.3f;
    [SerializeField] private float submenuOffset = 0.5f;
    [SerializeField] private float trashOffset = 0.18f;
    [SerializeField] private float emptyCupOffset = 0.5f;

    // Spawner
    public List<Spawner> spawners_burger = new List<Spawner>();
    public List<Spawner> spawners_subMenu = new List<Spawner>();

    public List<Counter> counters = new List<Counter>();
    public List<Trashbin> trashBins = new List<Trashbin>();

    #region Reference Properties 
    public DriveThruCounter DriveThruCounter { get; private set; }
    public PackageTable PackageTable { get; private set; }
    public int PaidAmount
    {
        get => data.PaidAmount;
        set => data.PaidAmount = value;
    }
    public int UpgradeCount
    {
        get => data.UpgradeCount;
        set => data.UpgradeCount = value;
    }
    public bool IsUpgradableCamMoving => upgradableCam.IsMoving;
    #endregion

    public event System.Action OnUpgrade;
    public event System.Action<float> OnUnlock;

    public StoreData data;
    public CameraController upgradableCam;

    private PlayerController player;
    private string storeName;

    void Awake()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 30;
#endif
        instance = this;
        storeName = "store name";
        data = SaveLoadManager.LoadData<StoreData>(storeName);
        if (data == null)
        {
            data = new StoreData(storeName, startingMoney);
        }
        AdjustMoney(0);
        CalculateReward();

        for (int i = 0; i < data.EmployeeAmount; i++)
        {
            SpawnEmployee();
        }
    }

    private void Start()
    {
        counters = GameObject.FindObjectsOfType<Counter>(true).ToList();
        trashBins = GameObject.FindObjectsOfType<Trashbin>().ToList();

        var spawnerObjs = GameObject.FindObjectsOfType<Spawner>(true);
        foreach (Spawner spawner in spawnerObjs)
        {
            if (spawner.type == eObjectType.HAMBURGER)
            {
                spawners_burger.Add(spawner);
            }
            else if (spawner.type == eObjectType.SUBMENU)
            {
                spawners_subMenu.Add(spawner);
            }
        }

        DriveThruCounter = GameObject.FindObjectOfType<DriveThruCounter>(true);
        PackageTable = GameObject.FindObjectOfType<PackageTable>(true);

        upgradableCam = GameObject.FindObjectOfType<CameraController>();
        player = GameObject.FindObjectOfType<PlayerController>();

        for (int i = 0; i < UpgradeCount; ++i)
        {
            upgradables[i].Upgrade(false);
        }

        UpdateUpgradeButton();

        if (UpgradeCount == 0)
        {
            player.transform.position = new Vector3(-3.75f, 0.16f, -21.5f);
        }
        else
        {
            player.transform.position = new Vector3(0f, 0.16f, 0f);
        }
    }

    private void OnApplicationQuit()
    {
        ExitGame();
    }
    private void CalculateReward()
    {
        DateTime LastTime = data.LastTime;
        DateTime current = DateTime.Now;

        TimeSpan time = current - LastTime;
#if UNITY_EDITOR
        Debug.Log(time.Hours + "hour " + time.Minutes + "minutes" + time.Seconds + "seconds 만의 재접속입니다.");
#endif

        offlineReward.SetRewardText(time.Hours * 1000 + time.Minutes * 100 + time.Seconds * 1);
    }
    public void ReceiveReward()
    {
        AdjustMoney(offlineReward.rewardNow);
        offlineReward.ReceiveReward();

        sceneFader.FadeOut(() =>
        {
            upgradableCam.waitDuration = 0.0f;
            Vector3 upgradablePosition = upgradables[UpgradeCount].BuyingPosition;
            upgradableCam.ShowPosition(upgradablePosition);
        });
    }

    public void ExitGame()
    {
        data.LastTime = DateTime.Now;
        SaveLoadManager.SaveData<StoreData>(data, storeName);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void LoadDataFromCSV<T>(string filename, List<T> dataLst, Func<string[], T> parser)
    {
        TextAsset csvData = Resources.Load<TextAsset>(filename);
        if (csvData == null)
        {
#if UNITY_EDITOR
            Debug.LogError("CSV is not founded: " + filename);
#endif
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

    private void UpdateUpgradeButton()
    {
        if (upgradables.Count == 0)
        {
#if UNITY_EDITOR
            Debug.LogWarning("There are no upgradables in the scene. Please add the upgradables to proceed.");
#endif
            return;
        }

        if (UpgradeCount < upgradables.Count)
        {
            var upgradable = upgradables[UpgradeCount];
            upgradeButton.transform.position = upgradable.BuyingPosition;
            int price = Mathf.RoundToInt(Mathf.Round(baseUnlockPrice * Mathf.Pow(unlockGrowthFactor, UpgradeCount)) / 5.0f) * 5;
            upgradeButton.Initialize(price, PaidAmount);
        }
        else
        {
            data.IsUnlocked = true;
            upgradeButton.gameObject.SetActive(false);
        }

        float progress = UpgradeCount / (float)upgradables.Count;
        OnUnlock?.Invoke(progress);
    }

    public void BuyUpgradable()
    {
        /// upgrade와 effect 재생, 카메라 이동, 그리고 save
        upgradables[UpgradeCount].Upgrade();
        // entrance unlock 시 애니메이션 재생
        if (UpgradeCount == 0 || UpgradeCount == 51)
        {
            upgradableCam.waitDuration = 3.0f;
            FindObjectOfType<PlayerController>().Animator.SetTrigger("unlockEntrance");
        }
        UpgradeCount++;
        PaidAmount = 0;
        UpdateUpgradeButton();

        // effect, sound
        upgradeParticle.transform.position = upgradables[UpgradeCount - 1].transform.position;
        upgradeParticle.Play();
        SoundManager.PlaySFX("SFX_upgrade");

        // camera move
        Vector3 upgradablePosition = upgradables[UpgradeCount].BuyingPosition;
        upgradableCam.ShowPosition(upgradablePosition);

        // save
        SaveLoadManager.SaveData<StoreData>(data, storeName);
    }

    public void AdjustMoney(int value)
    {
        data.Money += value;
        displayMoneyText.text = GetFormattedMoney(data.Money);
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

        SoundManager.PlaySFX("SFX_upgradeButton");

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
                foreach (var upgradable in upgradables)
                {
                    upgradable.UpgradeStats();
                }
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

    public float GetStackOffset(eObjectType type)
    {
        switch (type)
        {
            case eObjectType.HAMBURGER:
                return burgerOffset;
            case eObjectType.BURGERPACK:
                return burgerPackOffset;
            case eObjectType.SUBMENU:
                return submenuOffset;
            case eObjectType.TRASH:
                return trashOffset;
            case eObjectType.EMPTYCUP:
                return emptyCupOffset;
            default:
                return 0.0f;
        }
    }

    public OrderInfo GetOrderInfo(int index)
    {
        return orderInfo[index];
    }

    public KioskOrderInfo GetKioskOrderInfo(int index)
    {
        return kioskOrderInfo[index];
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