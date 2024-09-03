using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpawner : MonoBehaviour
{
    private GameObject customer;
    private Line lineQueue;
    private int spawnedCustomerCount = 0;
    [SerializeField]
    private float spawnCoolDownTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        customer = Resources.Load("Prefabs/Customer/Customer") as GameObject;
        lineQueue = GameObject.Find("LineQueue").GetComponent<Line>();
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
            if (spawnedCustomerCount < lineQueue.GetMaxQueueCount())
            {
                GameObject obj = MonoBehaviour.Instantiate(customer);
                obj.SetActive(true);
                obj.transform.position = transform.position;
                ++spawnedCustomerCount;
            }
            yield return new WaitForSeconds(spawnCoolDownTime);
        }
    }
}
