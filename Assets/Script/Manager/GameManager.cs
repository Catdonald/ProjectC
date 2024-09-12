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

    [System.Serializable]
    public struct LevelData
    {
        public float moveSpeed;
        public int maxCapacity;
    }

    [Header("# level data for all objects")]
    public List<LevelData> playerLevelData = new List<LevelData>();
    public List<LevelData> staffLevelData = new List<LevelData>();

    // 레벨 관리가 필요한 오브젝트들

    [Header("# 플레이어")]
    public GameObject Player;

    [Header("# 직원")]
    public List<GameObject> staffs;

    [Header("# 조리대")]
    public List<GameObject> cookers;

    [Header("# 테이블")]
    public List<GameObject> tables;


    void Awake()
    {
        instance = this;
        LoadData();
    }

    private void Start()
    {
        
    }

    void LateUpdate()
    {
        AutoSave();
    }

    /// <summary>
    /// 데이터 로드, 세이브 함수
    /// </summary>
    
    private void AutoSave()
    {
        GameTime += Time.deltaTime;
        
        if(GameTime == 10f)
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
        LoadDataFromCSV("PlayerLevelData", playerLevelData, ParseLevelData);
        LoadDataFromCSV("StaffLevelData", staffLevelData, ParseLevelData);

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
