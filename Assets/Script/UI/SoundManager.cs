using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound Management")]
    public float soundVol;
    public List<AudioSource> sounds_3d;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private AudioSource pitchSound;

    private Dictionary<string, AudioClip> bgms;
    private Dictionary<string, AudioClip> sfxs;

    [Header("Pitch")]
    [SerializeField] private float pitchIncrease = 0.1f; // 피치가 증가할 양
    [SerializeField] private float maxPitch = 4.0f;     // 피치가 최대 도달할 값
    [SerializeField] private float initialPitch = 1.0f; // 초기 피치 값
    [SerializeField] private string soundName;

    void Awake()
    {
        soundVol = 0.5f;

        bgms = new Dictionary<string, AudioClip>();
        sfxs = new Dictionary<string, AudioClip>();
    }
    void Start()
    {
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
        StopSFX();
    }

    public void PlayBGM(string name)
    {
        bgm.clip = bgms[name];
        bgm.Play();
    }
    public void StopBGM()
    {
        bgm.Stop();
    }
    public void StopSFX()
    {
        sfx.Stop();
    }
    public void PlaySFX(string name)
    {
        sfx.clip = sfxs[name];
        sfx.PlayOneShot(sfx.clip);
    }
    public void PlayPitch(string name)
    {
        pitchSound.clip = sfxs[name];
        pitchSound.PlayOneShot(pitchSound.clip);
    }

    public void SetVolume(float vol)
    {
        for (int i = 0; i < sounds_3d.Count; i++)
        {
            sounds_3d[i].volume = vol;
        }

        bgm.volume = vol;
        sfx.volume = vol;
        pitchSound.volume = vol;
    }
    public float GetSFXSoundLength(string name)
    {
        return sfxs[name].length;
    }

    public void PlayPitchSound(string name)
    {
        PlayPitch(name);
        Invoke(nameof(OnSoundComplete), GetSFXSoundLength(name));
    }

    private void OnSoundComplete()
    {
        if (pitchSound.pitch < maxPitch)
        {
            pitchSound.pitch += pitchIncrease;
        }

        PlayPitchSound(pitchSound.clip.name);
    }
    public void QuitPitchSound()
    {
        CancelInvoke(nameof(OnSoundComplete));
        pitchSound.pitch = initialPitch;
    }
}
