using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 240822 
/// 오브젝트를 생산할 수 있는 모든 객체
/// </summary>
public class Spawner : Stackable
{
    void Awake()
    {
        type = eObjectType.HAMBURGER;
    }

    void Start()
    {
        GameObject obj = GameManager.instance.PoolManager.Get((int)type);
        objectHeight = obj.GetComponent<Renderer>().bounds.size.y;
        GameManager.instance.PoolManager.Return(obj);
        actingTime = 5;
    }

    void Update()
    {
        if (stack.Count < 5 && !isActing)
        {
            isActing = true;
            StartCoroutine(SpawnObject((int)type));
        }
    }

    public override void Enter(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;
        base.Enter(other);
        player = other.transform.GetComponentInChildren<playerStack>();
    }

    public override void Interaction(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            return;

        if (stack.Count == 0)
            return;

        if (player.stack.Count == 0 && player.type == eObjectType.LAST)
            player.type = type;

        if (player.type == type)
            player.ReceiveObject(stack.Pop(), type, objectHeight);
    }
}