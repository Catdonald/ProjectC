using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerMachine : FoodMachine
{
    Spawner burgerSpawner;

    // Start is called before the first frame update
    void Start()
    {
        burgerSpawner = GetComponentInChildren<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        /// 임시코드.
        if(Input.GetKeyDown(KeyCode.B))
        {
            Upgrade();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        burgerSpawner.Enter(collision);
    }
    void OnCollisionStay(Collision collision)
    {
        burgerSpawner.Interaction(collision);
    }
}
