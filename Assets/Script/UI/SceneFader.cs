using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SceneFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.0f;
    private Image fadeImage;

    void Awake()
    {
        fadeImage = GetComponent<Image>();
    }

    public void FadeIn(System.Action onComplete = null)
    {
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.raycastTarget = true;
        fadeImage.DOFade(1f, fadeDuration).OnComplete(() => onComplete?.Invoke());
    }

    public void FadeOut(System.Action onComplete = null)
    {
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = false;
        fadeImage.DOFade(0f, fadeDuration).OnComplete(() => onComplete?.Invoke());
    }
}
