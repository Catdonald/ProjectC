using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    [SerializeField] private MoneyPile firstGivingMoney;
    [SerializeField] private Image arrow;
    [SerializeField] private TextMeshProUGUI nextStep;
    public List<Transform> tutorialpositions = new List<Transform>();
    [SerializeField] string[] explain;

    [Header("Ref for Tutorial")]
    [SerializeField] private PlayerController playercontroller;
    [SerializeField] private Table table;

    private bool isTutorialEnd = false;

    void Start()
    {
        if (GameManager.instance.data.TutorialCount == 0)
        {
            for (int i = 0; i < 8; ++i)
                firstGivingMoney.AddMoney();
        }
        else
        {
            firstGivingMoney.gameObject.SetActive(false);
        }

        if(GameManager.instance.data.TutorialCount < tutorialpositions.Count - 1)
            nextStep.text = explain[GameManager.instance.data.TutorialCount];
        else
            nextStep.gameObject.SetActive(false);

        StartCoroutine(TutorialSequence(GameManager.instance.data.TutorialCount));
    }

    public void ShowNextStep()
    {
        ++GameManager.instance.data.TutorialCount;
        nextStep.text = explain[GameManager.instance.data.TutorialCount];
    }

    public void Update()
    {
        if (!isTutorialEnd)
            arrow.transform.position = Camera.main.WorldToScreenPoint(tutorialpositions[GameManager.instance.data.TutorialCount].position + new Vector3(0.5f, 2, 0));
    }

    IEnumerator TutorialSequence(int tutorialCount)
    {
        switch (tutorialCount)
        {
            case 0:
                {
                    // start
                    yield return new WaitUntil(() => firstGivingMoney.Count == 0);
                    ShowNextStep();
                    goto case 1;
                }
            case 1:
                {
                    yield return new WaitUntil(() => GameManager.instance.UpgradeCount == 1);
                    ShowNextStep();
                    goto case 2;
                }
            case 2:
                {
                    yield return new WaitUntil(() => GameManager.instance.UpgradeCount == 2);
                    ShowNextStep();
                    goto case 3;
                }
            case 3:
                {
                    yield return new WaitUntil(() => GameManager.instance.UpgradeCount == 3);
                    ShowNextStep();
                    goto case 4;
                }
            case 4:
                {
                    yield return new WaitUntil(() => GameManager.instance.UpgradeCount == 4);
                    ShowNextStep();
                    goto case 5;
                }
            case 5:
                {
                    // put burger
                    yield return new WaitUntil(() => playercontroller.Stack.Count > 0 && playercontroller.Stack.StackType == eObjectType.HAMBURGER);
                    ShowNextStep();
                    goto case 6;
                }
            case 6:
                {
                    // move burger
                    yield return new WaitUntil(() => playercontroller.Stack.Count == 0);
                    ShowNextStep();
                    goto case 7;
                }
            case 7:
                {
                    // sell burger
                    yield return new WaitUntil(() => table.isTrashActive);
                    ShowNextStep();
                    goto case 8;
                }
            case 8:
                {
                    // refresh table
                    yield return new WaitUntil(() => !table.isTrashActive && playercontroller.Stack.StackType == eObjectType.TRASH);
                    ShowNextStep();
                    goto case 9;
                }
            case 9:
                {
                    // trash
                    yield return new WaitUntil(() => playercontroller.Stack.StackType == eObjectType.LAST);
                    EndTutorial();
                }
                break;
            default:
                {
                    EndTutorial();
                }
                break;
        }
    }

    void EndTutorial()
    {
        isTutorialEnd = true;
        arrow.gameObject.SetActive(false);
        nextStep.gameObject.SetActive(false);
    }
}
