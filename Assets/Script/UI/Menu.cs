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

    [SerializeField] private Image hapticBackgroundImage;
    [SerializeField] private Image hapticSliderImage;
    [SerializeField] private Text hapticOnText;
    [SerializeField] private Text hapticOffText;
    [SerializeField] private Text versionInfoText;

    private const int hapticOnSliderPosX = 155;
    private const int hapticOffSliderPosX = 50;
    private Color hapticOnBgColor = new Color(181 / 255.0f, 218 / 255.0f, 234 / 255.0f, 1.0f);
    private Color hapticOffBgColor = new Color(171 / 255.0f, 193 / 255.0f, 203 / 255.0f, 1.0f);

    void Start()
    {
        soundSlider.value = GameManager.instance.SoundVolume / 100.0f;
        bool isHapticOn = GameManager.instance.IsHapticOn;
        SettingHaptic(isHapticOn);
        versionInfoText.text = "version " + Application.version;
        settingsPanel.SetActive(false);
    }
    void Update()
    {
        GameManager.instance.SoundManager.SetVolume(soundSlider.value);
        GameManager.instance.SoundVolume = (int)(soundSlider.value * 100);
    }
    public void SetMenuCanvasOn(bool isOn)
    {
        settingsPanel.SetActive(isOn);

        if (isOn)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    
    private void SettingHaptic(bool isHapticOn)
    {
        GameManager.instance.IsHapticOn = isHapticOn;
        if(isHapticOn)
        {
            hapticOnText.gameObject.SetActive(true);
            hapticOffText.gameObject.SetActive(false);
            hapticBackgroundImage.color = hapticOnBgColor;
            hapticSliderImage.rectTransform.localPosition = new Vector3(hapticOnSliderPosX, hapticSliderImage.rectTransform.localPosition.y, hapticSliderImage.rectTransform.localPosition.z);
        }
        else
        {
            hapticOnText.gameObject.SetActive(false);
            hapticOffText.gameObject.SetActive(true);
            hapticBackgroundImage.color = hapticOffBgColor;
            hapticSliderImage.rectTransform.localPosition = new Vector3(hapticOffSliderPosX, hapticSliderImage.rectTransform.localPosition.y, hapticSliderImage.rectTransform.localPosition.z);
        }
    }

    public void OnClickHapticSettingButton()
    {
        bool hapticSetting = !GameManager.instance.IsHapticOn;
        SettingHaptic(hapticSetting);
    }
}
