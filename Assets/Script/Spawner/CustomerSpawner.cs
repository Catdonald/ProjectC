using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpawner : MonoBehaviour
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
            // ·£´ý¼ýÀÚ »Ì±â
            // switch(0) -> burgerJoint
            if (!lineQueue.IsQueueFull())
            {
                GameObject obj = GameManager.instance.PoolManager.Get(1);
                obj.transform.position = transform.position;
                obj.transform.forward = transform.forward;
                int randomValue = Random.Range(0, materials.Length);
                obj.GetComponent<SkinnedMeshRenderer>().material = materials[randomValue];
                lineQueue.QueueCount++;
                CustomerController customer = obj.GetComponent<CustomerController>();
                customer.entrance = GameObject.Find("Entrance_Point1");
                customer.spawner = gameObject;
                customer.line = lineQueue;
                customer.orderInfo = GameManager.instance.BurgerOrderInfo;
            }
            // switch(1)
            yield return new WaitForSeconds(spawnCoolDownTime);
        }
    }
}
