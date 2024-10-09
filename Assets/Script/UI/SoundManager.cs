using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float soundVol;
    public List<AudioSource> sounds;
    public AudioSource bgm;

    void Awake()
    {
        soundVol = 0.5f;
    }
    void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.loop = true;

        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudio)
        {
            sounds.Add(audioSource);
        }

        SetVolume(soundVol);

        bgm.Play();
    }

    public void SetVolume(float vol)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            sounds[i].volume = vol;
        }

        bgm.volume = vol;
    }
}
