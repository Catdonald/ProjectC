using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 240821 오수안
/// 게임매니저, 싱글턴 
/// 게임 매니저를 통해 다른 매니저 인스턴스에 접근
/// </summary>


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PoolManager PoolManager;

    void Awake()
    {
        instance = this;
    }
}
