using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerMachine : FoodMachine
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /// �ӽ��ڵ�.
        if(Input.GetKeyDown(KeyCode.B))
        {
            Upgrade();
        }
    }
}
