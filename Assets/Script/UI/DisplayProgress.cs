using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayProgress : MonoBehaviour
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text progressText;

    private void Awake()
    {
        progressSlider = GetComponentInChildren<Slider>();
        progressText = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        GameManager.instance.OnUnlock += UpdateProgress;
    }

    private void OnEnable()
    {
        if (GameManager.instance)
        {
            GameManager.instance.OnUnlock += UpdateProgress;
        }
    }

    private void OnDisable()
    {
        GameManager.instance.OnUnlock -= UpdateProgress;
    }

    private void UpdateProgress(float progress)
    {
        progressSlider.value = progress;
        progressText.text = $"{progress * 100:0.##}%";
    }
}
