using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbin : Interactable
{
    [SerializeField] private float throwInterval = 0.05f;
    [SerializeField] private Vector3 throwOffset = Vector3.zero;

    private float throwTimer = 0f;

    private void Update()
    {
        if (player == null)
            return;
        throwTimer += Time.deltaTime;
        if(throwTimer >= throwInterval)
        {
            throwTimer = 0f;
            var thrownObj = player.Stack.RemoveFromStack();
            if (thrownObj == null)
                return;
            thrownObj.transform.DOJump(transform.TransformPoint(throwOffset), 5.0f, 1, 0.5f)
                .OnComplete(() =>
                {
                    GameManager.instance.PoolManager.Return(thrownObj);
                    GameManager.instance.SoundManager.PlaySFX("SFX_bin");
                    Vibration.Vibrate(100);
                });
        }
    }

    public void ThrowToBin(playerStack stack)
    {
        var thrownObj = stack.RemoveFromStack();
        thrownObj.transform.DOJump(transform.TransformPoint(throwOffset), 5.0f, 1, 0.5f)
                .OnComplete(() =>
                {
                    GameManager.instance.PoolManager.Return(thrownObj);
                });
    }
}
