using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 240821 오수안 <PoolManager>
/// 모든 오브젝트의 오브젝트 풀링을 담당
/// Inspector 상의 배열에 풀링하려는 오브젝트를 추가한다
/// </summary>

public class PoolManager : MonoBehaviour
{
    [Header("Pooling objects")]
    [Tooltip("add list of gameobject for pooling")]
    [SerializeField] private List<GameObject> prefabs;
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    void Start()
    {
        foreach (GameObject obj in prefabs)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < 20; i++)
            {

                GameObject newObj = Instantiate(obj, transform);
                newObj.name = obj.name;
                newObj.SetActive(false);
                pool.Enqueue(newObj);
            }
            poolDictionary.Add(obj.name, pool);
        }
    }

    public GameObject SpawnObject(string prefabName)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogWarning("PoolManager does not contain " + prefabName);
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[prefabName];
        if (objectPool.Count == 0)
        {
            GameObject newObj = Instantiate(prefabs.FirstOrDefault(x => x.name == prefabName), transform);
            newObj.name = prefabName;
            return newObj;
        }

        GameObject obj = objectPool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public GameObject SpawnObject(int index)
    {
        if(index >= poolDictionary.Count)
        {
            return null;
        }

        string prefabName = poolDictionary.ElementAt(index).Key;
        Queue<GameObject> objectPool = poolDictionary.ElementAt(index).Value;
        if (objectPool.Count == 0)
        {
            GameObject newObj = Instantiate(prefabs.FirstOrDefault(x => x.name == prefabName), transform);
            newObj.name = prefabName;
            return newObj;
        }

        GameObject obj = objectPool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        string prefabName = obj.name;
        if(poolDictionary.ContainsKey(prefabName))
        {
            poolDictionary[prefabName].Enqueue(obj);
        }
    }
}
