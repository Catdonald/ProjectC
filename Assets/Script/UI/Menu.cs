using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject settingsPanel;  // 설정창 패널
    public Slider soundSlider;
    public Button quitButton;

    public Button enterMenu;
    public Button exitMenu;

    void Start()
    {
        soundSlider.value = GameManager.instance.SoundManager.soundVol;
        settingsPanel.SetActive(false);
    }
    void Update()
    {
        GameManager.instance.SoundManager.SetVolume(soundSlider.value);
    }
    public void SetMenuCanvasOn(bool isOn)
    {
        settingsPanel.SetActive(isOn);

        if (isOn)
            GameManager.instance.isStaticMode = true;
        else
            GameManager.instance.isStaticMode = false;
    }
    public void ExitGame()
    {
        
    }
}
