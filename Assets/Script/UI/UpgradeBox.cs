using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBox : MonoBehaviour
{
    public bool isPushed = false;
    private Image moneyGaugeUI;
    [SerializeField]
    float fillSpeed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            if(image.gameObject.name == "MoneyGauge")
            {
                moneyGaugeUI = image;
                break;
            }
        }
        moneyGaugeUI.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPushed)
        {
            moneyGaugeUI.fillAmount += fillSpeed * Time.deltaTime;
        }

        if(moneyGaugeUI.fillAmount >= 1.0f)
        {
            gameObject.SetActive(false);
        }
    }
}
