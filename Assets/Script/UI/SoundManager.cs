using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float soundVol;
    public List<AudioSource> sounds;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip[] sfxClips;

    private Dictionary<string, AudioClip> bgms;
    private Dictionary<string, AudioClip> sfxs;

    void Awake()
    {
        soundVol = 0.5f; 

        bgms = new Dictionary<string, AudioClip>();
        sfxs = new Dictionary<string, AudioClip>();
    }
    void Start()
    {
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudio)
        {
            sounds.Add(audioSource);
        }

        for (int i = 0; i < bgmClips.Length; i++)
        {
            bgms.Add(bgmClips[i].name, bgmClips[i]);
        }

        for (int i = 0; i < sfxClips.Length; i++)
        {
            sfxs.Add(sfxClips[i].name, sfxClips[i]);
        }

        SetVolume(soundVol);

        PlayBGM("BGM_main");
    }

    public void PlayBGM(string name)
    {
        bgm.clip = bgms[name];
        bgm.Play();
    }
    public void PlaySFX(string name, bool isLoop = false)
    {
        sfx.clip = sfxs[name];
        
        if(isLoop)
        {
            sfx.loop = true;
        }
        else
        {
            sfx.loop = false;
        }

        sfx.Play();
    }

    public void SetVolume(float vol)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            sounds[i].volume = vol;
        }

        bgm.volume = vol;
        sfx.volume = vol;
    }
}
