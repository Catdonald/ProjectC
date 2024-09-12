using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 240822 오수안
/// 플레이어의 버거를 가져가는 오브젝트 유형
/// </summary>

public class Receiver : Stackable
{
    void Awake()
    {
        type = eObjectType.HAMBURGER;
    }
    public override void Enter(Collision collision)
    {
        player = collision.gameObject.GetComponentInChildren<playerStack>();
    }
    public override void Interaction(Collision other)
    {
        if(player.stack.Count > 0 && player.type == type)
        {
            ReceiveObject(player.RequestObject(), player.type, player.objectHeight);
        }
    }
}
