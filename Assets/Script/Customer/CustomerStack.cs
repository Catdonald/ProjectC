using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStack : MonoBehaviour
{
    public Stack<GameObject> stack;

    bool isSeating;

    void Awake()
    {
        stack = new Stack<GameObject>();
    }


}
