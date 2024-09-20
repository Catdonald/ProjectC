using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerMachine : FoodMachine
{
    // Update is called once per frame
    void Update()
    {
        /// 임시코드.
        if(Input.GetKeyDown(KeyCode.B))
        {
            Upgrade();
        }
    }
}
