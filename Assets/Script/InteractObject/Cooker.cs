using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    public Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponentInChildren<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
