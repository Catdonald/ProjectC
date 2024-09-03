using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public PoolManager PoolManager;

    public GameObject idText;
    public GameObject levelText;
    public GameObject speedText;


    // �ӽ� ������
    info info;
    void Awake()
    {
        instance = this;
    }

    public void ChangeInfo()
    {
        info.id = Random.Range(1, 100);
        info.name = "suan";
        info.level = Random.Range(1, 100);
        info.playerSpeed = Random.Range(1, 100);
        info.playerMaxStuff = Random.Range(1, 100);

        ChangeText();
    }

    public void GetData()
    {
        Debug.Log("Loads Data");
       
        info.id = PlayerPrefs.GetInt("id", 0);
        info.name = PlayerPrefs.GetString("name", "default");
        info.level = PlayerPrefs.GetInt("level", 0);
        info.playerSpeed = PlayerPrefs.GetInt("speed", 0);
        info.playerMaxStuff = PlayerPrefs.GetInt("maxStuff", 0);

        ChangeText();
    }

    public void SaveData()
    {
        Debug.Log("Save Data");

        PlayerPrefs.SetInt("id", info.id);
        PlayerPrefs.SetString("name", info.name);
        PlayerPrefs.SetInt("level", info.level);
        PlayerPrefs.SetInt("speed", info.playerSpeed);
        PlayerPrefs.SetInt("maxStuff", info.playerMaxStuff);

        PlayerPrefs.Save();
    }

    void ChangeText()
    {
        idText.GetComponent<Text>().text = info.id.ToString(); 
        levelText.GetComponent<Text>().text = info.level.ToString(); 
        speedText.GetComponent<Text>().text = info.playerSpeed.ToString(); 
    }
}
