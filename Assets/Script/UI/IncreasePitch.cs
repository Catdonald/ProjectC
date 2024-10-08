using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePitch : MonoBehaviour
{
    public AudioSource audioSource;   // 사운드를 재생할 AudioSource
    public float pitchIncrease = 0.1f; // 피치가 증가할 양
    public float maxPitch = 3.0f;     // 피치가 최대 도달할 값
    public float initialPitch = 1.0f; // 초기 피치 값

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public void PlaySound()
    {
        audioSource.Play();
        Invoke(nameof(OnSoundComplete), audioSource.clip.length);
    }

    private void OnSoundComplete()
    {
        if (audioSource.pitch < maxPitch)
        {
            audioSource.pitch += pitchIncrease;
        }

        PlaySound();
    }

    public void QuitSound()
    {
        CancelInvoke(nameof(OnSoundComplete));
        audioSource.Stop();
        audioSource.pitch = initialPitch;
    }
}
