using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    [SerializeField] private MoneyPile firstGivingMoney;
    [SerializeField] private Image arrow;
    [SerializeField] private GameObject direction;
    [SerializeField] private TextMeshProUGUI nextStep;
    public List<Transform> tutorialpositions = new List<Transform>();
    [SerializeField] string[] explain;

    [Header("Ref for Tutorial")]
    [SerializeField] private PlayerController playercontroller;
    [SerializeField] private Table table;
    [SerializeField] private CameraController cam;
    [SerializeField] private MoneyPile second;

    private bool isTutorialEnd = false;
    private float edgeBuffer = 20f;
    private bool isTweening = false;

    void Start()
    {
        if (GameManager.instance.data.TutorialCount == 0)
        {
            for (int i = 0; i < 4; ++i)
                firstGivingMoney.AddMoney();
        }

        if (GameManager.instance.data.TutorialCount < tutorialpositions.Count - 1)
        {
            nextStep.text = explain[GameManager.instance.data.TutorialCount];
            StartCoroutine(TutorialSequence(GameManager.instance.data.TutorialCount));
        }
        else
        {
            EndTutorial();
        }
    }

    public void ShowNextStep()
    {
        ++GameManager.instance.data.TutorialCount;
        nextStep.text = explain[GameManager.instance.data.TutorialCount];
    }

    public void Update()
    {
        if (!isTutorialEnd)
        {
            arrow.transform.position = Camera.main.WorldToScreenPoint(tutorialpositions[GameManager.instance.data.TutorialCount].position + new Vector3(0, 2, 0));

            if (!isTweening)
            {
                arrow.transform.DOMoveY(arrow.transform.position.y + 50f, 1)
                .SetLoops(-1, LoopType.Yoyo) 
                .SetEase(Ease.InOutSine);

                isTweening = true; 
            }

            // rotation
            Vector3 dir = arrow.transform.position - direction.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            direction.transform.rotation = Quaternion.Euler(0, 0, angle);

            if (IsUIVisible(arrow.transform) || cam.IsMoving)
            {
                direction.gameObject.SetActive(false);
            }
            else
            {
                direction.gameObject.SetActive(true);
            }
        }
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
            case 5:
            case 6:
                {
                    GameManager.instance.data.TutorialCount = 4;
                    nextStep.text = explain[GameManager.instance.data.TutorialCount];
                    yield return new WaitUntil(() => GameManager.instance.UpgradeCount == 4);
                    ShowNextStep();
                    // put burger
                    yield return new WaitUntil(() => playercontroller.Stack.Count > 0 && playercontroller.Stack.StackType == eObjectType.HAMBURGER);
                    ShowNextStep();
                    // move burger
                    yield return new WaitUntil(() => playercontroller.Stack.Count == 0);
                    ShowNextStep();
                    goto case 7;
                }
            case 7:
            case 8:
                {
                    GameManager.instance.data.TutorialCount = 7;
                    nextStep.text = explain[GameManager.instance.data.TutorialCount];
                    yield return new WaitUntil(() => second.Count > 0);
                    SetUIActive(false);
                    // sell burger
                    yield return new WaitUntil(() => table.isTrashActive);
                    SetUIActive(true);
                    ShowNextStep();
                    // refresh table
                    yield return new WaitUntil(() => !table.isTrashActive && playercontroller.Stack.StackType == eObjectType.TRASH);
                    ShowNextStep();
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
        SetUIActive(false);
        firstGivingMoney.gameObject.SetActive(false);
    }

    bool IsUIVisible(Transform uiElement)
    {
        if (uiElement.position.x < 0 || uiElement.position.x > Screen.width || uiElement.position.y < 0 || uiElement.position.y > Screen.height)
        {
            return false; // 경계를 벗어난 경우
        }

        return true; // 모든 모서리가 화면 안에 있는 경우
    }

    private void SetUIActive(bool isActive)
    {
        arrow.gameObject.SetActive(isActive);
        nextStep.gameObject.SetActive(isActive);
        direction.gameObject.SetActive(isActive);
    }
}
