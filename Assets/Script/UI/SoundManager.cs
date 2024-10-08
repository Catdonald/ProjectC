using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private float sfxVol;
    [SerializeField] private float bgmVol;
    public List<AudioSource> sfx;
    public AudioSource bgm;

    void Awake()
    {
        sfxVol = 50;
        bgmVol = 50;
    }
    void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.loop = true;

        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudio)
        {
            if (audioSource.CompareTag("BGM"))
                continue;

            sfx.Add(audioSource);
        }

        SetSFXVolume(sfxVol);
        SetBGMVolume(bgmVol);

        bgm.Play();
    }

    public void SetSFXVolume(float vol)
    {
        for (int i = 0; i < sfx.Count; i++)
        {
            sfx[i].volume = vol;
        }
    }

    public void SetBGMVolume(float vol)
    {
        bgm.volume = vol;
    }
}
