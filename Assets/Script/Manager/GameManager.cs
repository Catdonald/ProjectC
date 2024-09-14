using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

/// <summary>
/// 240821 오수안
/// 게임매니저, 싱글턴 
/// 게임 매니저를 통해 다른 매니저 인스턴스에 접근
/// </summary>

// prefab test 용 임시 데이터
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

    [Header("# 매니저 클래스")]
    public PoolManager PoolManager;
    public TableManager TableManager;

    [Header("# 플레이어 정보")]
    public GameObject Player;
    public List<PlayerData> playerLevelData = new List<PlayerData>();

    [Header("# 직원 정보")]
    public List<GameObject> staffs = new List<GameObject>();
    public List<PlayerData> staffLevelData = new List<PlayerData>();

    [Header("# 조리대 정보")]
    public List<GameObject> cookers = new List<GameObject>();
    public List<CookerData> cookerLevelData = new List<CookerData>();

    [Header("# 카운터 정보")]
    public List<GameObject> counters = new List<GameObject>();

    [Header("# 테이블 정보")]
    public List<TableData> tableLevelData = new List<TableData>();

    [Header("# 해제 가능 오브젝트")]
    [SerializeField] private List<Unlockable> unlockables;

    [Header("# UI")]
    [SerializeField] private OrderInfo burgerOrderInfo;

    [Header("# Stack Offset")]
    [SerializeField] private float burgerOffset = 0.35f;
    [SerializeField] private float trashOffset = 0.18f;
    [SerializeField] private float burgerPackOffset = 0.3f;
    [SerializeField] private float coffeeOffset = 0.35f;

    public List<ObjectPile> TrashPiles {  get; private set; } = new List<ObjectPile>();

    // 요리기계 Spawner
    public List<Spawner> spawners_burger = new List<Spawner>();
    public List<Spawner> spawners_burgerPack = new List<Spawner>();
    //public List<Spawner> spawners_coffee = new List<Spawner>();

    // 카운터 Receiver
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
    /// 데이터 로드, 세이브 함수
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
        Debug.Log("데이터 저장");
    }
    private void LoadData()
    {
        LoadDataFromCSV("PlayerLevelData", playerLevelData, ParsePlayerData);
        LoadDataFromCSV("CookerLevelData", cookerLevelData, ParseCookerData);
        LoadDataFromCSV("TableLevelData", tableLevelData, ParseTableData);

        Debug.Log("데이터 로드");
    }

    void LoadDataFromCSV<T>(string filename, List<T> dataLst, Func<string[], T> parser)
    {
        TextAsset csvData = Resources.Load<TextAsset>(filename);
        if (csvData == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + filename);
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
    private PlayerData ParsePlayerData(string[] values)
    {
        PlayerData data = new PlayerData
        {
            Level = int.Parse(values[0]),
            moveSpeed = float.Parse(values[1]),
            maxCapacity = int.Parse(values[2])
        };

        return data;
    }
    private CookerData ParseCookerData(string[] values)
    {
        CookerData data = new CookerData
        {
            Level = int.Parse(values[0]),
            spawnSpeed = float.Parse(values[1]),
            maxCapacity = int.Parse(values[2])
        };

        return data;
    }
    private TableData ParseTableData(string[] values)
    {
        TableData data = new TableData
        {
            Level = int.Parse(values[0]),
            eatSpeed = float.Parse(values[1]),
            price = int.Parse(values[2])
        };

        return data;
    }

    // cooker 추가될 때 호출
    public void AddCooker()
    {
        //cook
    }

    // test code
    //public GameObject idText;
    //public GameObject levelText;
    //public GameObject speedText;

    //// 임시 데이터
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
