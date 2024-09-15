using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

/// <summary>
/// 240821 ������
/// ���ӸŴ���, �̱��� 
/// ���� �Ŵ����� ���� �ٸ� �Ŵ��� �ν��Ͻ��� ����
/// </summary>

// prefab test �� �ӽ� ������
struct info
{
    public int id;
    public string name;
    public int level;
    public int playerSpeed;
    public int playerMaxStuff;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Info")]
    public float GameTime;
    public int money { get; set; }
    public int level;
    public int exp;
    public int[] nextEXP;

    [Header("# �Ŵ��� Ŭ����")]
    public PoolManager PoolManager;
    public TableManager TableManager;

    [System.Serializable]
    public struct LevelData
    {
        public float moveSpeed;
        public int maxCapacity;
    }

    [Header("# level data for all objects")]
    public List<LevelData> playerLevelData = new List<LevelData>();
    public List<LevelData> staffLevelData = new List<LevelData>();

    // ���� ������ �ʿ��� ������Ʈ��

    [Header("# �÷��̾�")]
    public GameObject Player;

    [Header("# ����")]
    public List<GameObject> staffs;

    [Header("# ������")]
    public List<GameObject> upgrades;


    [Header("# ���� ���� ������Ʈ")]
    [SerializeField] private List<Unlockable> unlockables;

    [Header("# UI")]
    [SerializeField] private OrderInfo burgerOrderInfo;

    [Header("# Stack Offset")]
    [SerializeField] private float burgerOffset = 0.35f;
    [SerializeField] private float trashOffset = 0.18f;
    [SerializeField] private float burgerPackOffset = 0.3f;
    [SerializeField] private float coffeeOffset = 0.35f;

    public List<ObjectPile> TrashPiles {  get; private set; } = new List<ObjectPile>();

    // �丮��� Spawner
    public List<Spawner> spawners_burger = new List<Spawner>();
    public List<Spawner> spawners_burgerPack = new List<Spawner>();
    //public List<Spawner> spawners_coffee = new List<Spawner>();

    // ī���� Receiver
    public Receiver receiver_burger;
    public Receiver receiver_burgerPack;
    //Receiver receiver_coffee;

    #region Reference Properties
    public OrderInfo BurgerOrderInfo => burgerOrderInfo;
    public TrashBin TrashBin { get; private set; }
    #endregion

    void Awake()
    {
        instance = this;
        money = 50;
        LoadData();
    }

    private void Start()
    {
        GameObject[] counterObjs = GameObject.FindGameObjectsWithTag("Counter");
        foreach (GameObject counter in counterObjs)
        {
            counters.Add(counter);
        }

        GameObject[] cookerObjs = GameObject.FindGameObjectsWithTag("Cooker");
        foreach (GameObject cooker in cookerObjs)
        {
            cookers.Add(cooker);
        }

        var spawnerObjs = GameObject.FindObjectsOfType<Spawner>();
        foreach (Spawner spawner in spawnerObjs)
        {
            if (spawner.objectType == StackType.BURGER)
            {
                spawners_burger.Add(spawner);
            }
            else if (spawner.objectType == StackType.BURGERPACK)
            {
                spawners_burgerPack.Add(spawner);
            }
        }

        var receiverObjs = GameObject.FindObjectsOfType<Receiver>();
        foreach (Receiver receiver in receiverObjs)
        {
            if (receiver.objectType == StackType.BURGER)
            {
                receiver_burger = receiver;
            }
            else if (receiver.objectType == StackType.BURGERPACK)
            {
                receiver_burgerPack = receiver;
            }
        }

        var objectPiles = GameObject.FindObjectsOfType<ObjectPile>();
        foreach(var objectPile in objectPiles)
        {
            if(objectPile.StackType == StackType.TRASH)
            {
                TrashPiles.Add(objectPile);
            }
            // burgerPiles
            // burgerPackPiles
        }

        TrashBin = GameObject.FindObjectOfType<TrashBin>();
    }

    void LateUpdate()
    {
        AutoSave();
    }

    public float GetStackOffset(StackType stackType)
    {
        float offset = 0.0f;
        switch (stackType)
        {
            case StackType.NONE:
                offset = 0.0f; 
                break;
            case StackType.BURGER:
                offset = burgerOffset;
                break;
            case StackType.BURGERPACK:
                offset = burgerPackOffset;
                break;
            case StackType.COFFEE:
                offset = coffeeOffset;
                break;
            case StackType.TRASH:
                offset = trashOffset;
                break;
        }
        return offset;
    }

    /// <summary>
    /// ������ �ε�, ���̺� �Լ�
    /// </summary>

    private void AutoSave()
    {
        GameTime += Time.deltaTime;

        if (GameTime == 10f)
        {
            GameTime = 0;
            SaveData();
        }
    }

    public void SaveData()
    {
        Debug.Log("������ ����");
    }
    private void LoadData()
    {
        LoadDataFromCSV("PlayerLevelData", playerLevelData, ParseLevelData);
        LoadDataFromCSV("StaffLevelData", staffLevelData, ParseLevelData);

        Debug.Log("������ �ε�");
    }

    void LoadDataFromCSV<T>(string filename, List<T> dataLst, Func<string[], T> parser)
    {
        TextAsset csvData = Resources.Load<TextAsset>(filename);
        if (csvData == null)
        {
            Debug.LogError("CSV ������ ã�� �� �����ϴ�: " + filename);
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
    private LevelData ParseLevelData(string[] values)
    {
        LevelData data = new LevelData
        {
            moveSpeed = float.Parse(values[1]),
            maxCapacity = int.Parse(values[2])
        };

        return data;
    }
    //private CookerData ParseCookerData(string[] values)
    //{
    //    CookerData data = new CookerData
    //    {
    //        Level = int.Parse(values[0]),
    //        spawnSpeed = float.Parse(values[1]),
    //        maxCapacity = int.Parse(values[2])
    //    };

    //    return data;
    //}
    //private TableData ParseTableData(string[] values)
    //{
    //    TableData data = new TableData
    //    {
    //        Level = int.Parse(values[0]),
    //        eatSpeed = float.Parse(values[1]),
    //        price = int.Parse(values[2])
    //    };

    //    return data;
    //}

    // cooker �߰��� �� ȣ��
    public void AddCooker()
    {
        //cook
    }

    // test code
    //public GameObject idText;
    //public GameObject levelText;
    //public GameObject speedText;

    //// �ӽ� ������
    //info info;
    //public void ChangeInfo()
    //{
    //    info.id = Random.Range(1, 100);
    //    info.name = "suan";
    //    info.level = Random.Range(1, 100);
    //    info.playerSpeed = Random.Range(1, 100);
    //    info.playerMaxStuff = Random.Range(1, 100);

    //    ChangeText();
    //}

    //public void GetData()
    //{
    //    Debug.Log("Loads Data");

    //    info.id = PlayerPrefs.GetInt("id", 0);
    //    info.name = PlayerPrefs.GetString("name", "default");
    //    info.level = PlayerPrefs.GetInt("level", 0);
    //    info.playerSpeed = PlayerPrefs.GetInt("speed", 0);
    //    info.playerMaxStuff = PlayerPrefs.GetInt("maxStuff", 0);

    //    ChangeText();
    //}

    //public void SaveData()
    //{
    //    Debug.Log("Save Data");

    //    PlayerPrefs.SetInt("id", info.id);
    //    PlayerPrefs.SetString("name", info.name);
    //    PlayerPrefs.SetInt("level", info.level);
    //    PlayerPrefs.SetInt("speed", info.playerSpeed);
    //    PlayerPrefs.SetInt("maxStuff", info.playerMaxStuff);

    //    PlayerPrefs.Save();
    //}

    //void ChangeText()
    //{
    //    idText.GetComponent<Text>().text = info.id.ToString(); 
    //    levelText.GetComponent<Text>().text = info.level.ToString(); 
    //    speedText.GetComponent<Text>().text = info.playerSpeed.ToString(); 
    //}
}
