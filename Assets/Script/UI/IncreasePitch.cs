using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePitch : MonoBehaviour
{
    public AudioSource audioSource;   // ���带 ����� AudioSource
    public float pitchIncrease = 0.1f; // ��ġ�� ������ ��
    public float maxPitch = 3.0f;     // ��ġ�� �ִ� ������ ��
    public float initialPitch = 1.0f; // �ʱ� ��ġ ��

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
