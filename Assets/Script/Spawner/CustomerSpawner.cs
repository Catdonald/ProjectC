using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    private Line lineQueue;
    [SerializeField]
    private float spawnCoolDownTime = 5.0f;

    public Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        lineQueue = GameObject.Find("LineQueue").GetComponent<Line>();
        materials = new Material[4];
        for(int i = 0; i < materials.Length; i++)
        {
            string path = "Materials/CatMat" + (i + 1).ToString();
            materials[i] = Resources.Load<Material>(path);
        }
        StartCoroutine("SpawnCustomer");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnCustomer()
    {
        while (true)
        {
            if (!lineQueue.IsLineQueueFull())
            {
                //GameObject obj = MonoBehaviour.Instantiate(customer);
                GameObject obj = GameManager.instance.PoolManager.Get(1);
                obj.SetActive(true);
                obj.transform.position = transform.position;
                obj.transform.forward = transform.forward;
                int randomValue = Random.Range(0, materials.Length);
                obj.GetComponent<SkinnedMeshRenderer>().material = materials[randomValue]; 
                lineQueue.currentQueueCount++;
            }
            yield return new WaitForSeconds(spawnCoolDownTime);
        }
    }
}
