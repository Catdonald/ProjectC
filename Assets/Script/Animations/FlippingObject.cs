using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippingObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Flip();
    }

    private void Flip()
    {
        float delay = Random.Range(1f, 3f);
        var sequence = DOTween.Sequence();
        sequence.SetDelay(delay);
        sequence.Append(transform.DOJump(transform.position, 1f, 1, 0.5f));
        sequence.Join(transform.DOLocalRotate(new Vector3(0, 0, 180), 0.5f, RotateMode.LocalAxisAdd));
        sequence.OnComplete(() => Flip());
    }
}
