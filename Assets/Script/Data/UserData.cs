using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int SoundVolume { get; set; }
    public bool IsHapticOn { get; set; }

    public UserData()
    {
        SoundVolume = 100;
        IsHapticOn = true;
    }
}
