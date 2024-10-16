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

    private bool tutorialStart = false;
    private bool isTutorialEnd = false;
    private int tutorialCount = 0;

    void Start()
    {
        if (!tutorialStart)
        {
            for (int i = 0; i < 8; ++i)
                firstGivingMoney.AddMoney();

            nextStep.text = explain[tutorialCount];
        }
        else
        {
            firstGivingMoney.gameObject.SetActive(false);
        }

        if (isTutorialEnd)
        {
            arrow.gameObject.SetActive(false);
            nextStep.gameObject.SetActive(false);
        }

        StartCoroutine(TutorialSequence());
    }

    public void ShowNextStep()
    {
        ++tutorialCount;
        nextStep.text = explain[tutorialCount];
    }

    public void Update()
    {
        arrow.transform.position = Camera.main.WorldToScreenPoint(tutorialpositions[tutorialCount].position);
    }

    IEnumerator TutorialSequence()
    {
        // start
        yield return new WaitUntil(() => firstGivingMoney.Count == 0);
        ShowNextStep();

        // open burger shop
        // add counter
        // add burger machine
        // add table
        for (int i = 0; i < 4; ++i)
        {
            yield return new WaitUntil(() => GameManager.instance.UpgradeCount == i + 1);
            ShowNextStep();
        }

        // put burger
        yield return new WaitUntil(() => playercontroller.Stack.Count > 0 && playercontroller.Stack.StackType == eObjectType.HAMBURGER);
        ShowNextStep();

        // move burger
        yield return new WaitUntil(() => playercontroller.Stack.Count == 0);
        ShowNextStep();

        // sell burger
        yield return new WaitUntil(() => table.isTrashActive);
        ShowNextStep();

        // refresh table
        yield return new WaitUntil(() => !table.isTrashActive && playercontroller.Stack.StackType == eObjectType.TRASH);
        ShowNextStep();

        // trash
        yield return new WaitUntil(() => playercontroller.Stack.StackType == eObjectType.LAST);
        EndTutorial();
    }

    void EndTutorial()
    {
        arrow.gameObject.SetActive(false);
        nextStep.gameObject.SetActive(false);
    }
}
