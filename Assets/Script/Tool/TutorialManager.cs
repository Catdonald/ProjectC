using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image arrow;
    [SerializeField] private TextMeshProUGUI nextStep;

    [SerializeField] string[] explain;

    void Start()
    {
        if (GameManager.instance.isTutorialEnd)
        {
            arrow.gameObject.SetActive(false);
            nextStep.gameObject.SetActive(false);
        }
    }

    public void ShowNextStep()
    {
        if(GameManager.instance.isTutorialEnd)
        {
            arrow.gameObject.SetActive(false);
            nextStep.gameObject.SetActive(false);
        }

        var step = GameManager.instance.upgradables[GameManager.instance.UpgradeCount];
        if (step != null)
        {
            arrow.transform.position = Camera.main.WorldToScreenPoint(step.transform.position + new Vector3(0, 3, 0));
            nextStep.text = explain[GameManager.instance.UpgradeCount];
        }
    }
}
