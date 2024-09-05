using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    private GameObject customer;
    private Line lineQueue;
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
            if (!lineQueue.IsLineQueueFull())
            {
                //GameObject obj = MonoBehaviour.Instantiate(customer);
                GameObject obj = GameManager.instance.PoolManager.Get(1);
                obj.SetActive(true);
                obj.transform.position = transform.position;
                lineQueue.currentQueueCount++;
            }
            yield return new WaitForSeconds(spawnCoolDownTime);
        }
    }
}
