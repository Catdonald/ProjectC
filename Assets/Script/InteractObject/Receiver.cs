using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 240822 
/// 오브젝트를 받을 수 있는 모든 객체
/// </summary>

public class Receiver : Stackable
{
    void Awake()
    {
        
    }
    public override void Enter(Collision collision)
    {
        player = collision.gameObject.GetComponentInChildren<playerStack>();
    }
    public override void Interaction(Collision other)
    {
        if(player.Count > 0 && player.StackType == type && !IsFull)
        {
            ReceiveObject(player.RemoveFromStack(), type, GameManager.instance.GetStackOffset(type));
            GameManager.instance.SoundManager.PlaySFX("SFX_pop");
            Vibration.Vibrate(100);
        }
    } 
}
