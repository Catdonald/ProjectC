using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 240822
/// 플레이어, 직원 스택
/// </summary>

public class playerStack : Stackable
{
    void Awake()
    {
        type = eObjectType.LAST;
    }

    void Start()
    {

    }

    void Update()
    {
        if (stack.Count == 0)
        {
            type = eObjectType.LAST;
        }
    }

    public override void ReceiveObject(GameObject obj, eObjectType objType, float objHegiht)
    {
        if (stack.Count == 0 ||
            (stack.Count > 0 && objType == type))
        {
            base.ReceiveObject(obj, objType, objHegiht);
            type = objType;
        }
    }
}
