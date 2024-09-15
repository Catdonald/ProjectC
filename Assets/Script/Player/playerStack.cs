using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 240822 ������
/// �÷��̾ �̰� �ٴϴ� ���� ����
/// </summary>

public class playerStack : Stackable
{
    public PlayerData playerData;

    void Awake()
    {
        type = eObjectType.LAST;
    }

    void Start()
    {
        playerData = transform.parent.GetComponent<PlayerData>();
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
        base.ReceiveObject(obj, objType, objHegiht);
    }
}
