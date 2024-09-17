using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// 데이터 저장, 로드한다.
/// 이거 나중에 구글 연동하면서 바꿔야한다..
/// 일단 임시로 틀만 잡는다는 느낌으로 만든다.
/// </summary>
public static class SaveLoadManager
{
    public static void SaveData<T>(T data, string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName + ".dat";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static T LoadData<T>(string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName + ".dat";
        if(File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            T data = (T)formatter.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }
        else
        {
            return default(T);
        }
    }
}
