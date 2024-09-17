using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// ������ ����, �ε��Ѵ�.
/// �̰� ���߿� ���� �����ϸ鼭 �ٲ���Ѵ�..
/// �ϴ� �ӽ÷� Ʋ�� ��´ٴ� �������� �����.
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
