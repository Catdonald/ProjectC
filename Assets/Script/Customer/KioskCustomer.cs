using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class KioskCustomer : MonoBehaviour
{
    [SerializeField] private int maxOrderCount = 5;
    [SerializeField] private GameObject orderingBubbleUI;

    private Transform spawnPoint;
    private GameObject kiosk;
    private KioskOrderInfo orderInfo;
    private NavMeshAgent agent;
    private Animator animator;
    private MixingStack stack;
    private TextMeshProUGUI textUI;
    private bool startFlag = false;

    public int FirstOrderCount { get; set; }
    public int SecondOrderCount { get; set; }
    public bool HasOrder { get; set; }
    public bool FirstOrderDone { get; set; }
    public bool SecondOrderDone { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stack = GetComponentInChildren<MixingStack>();
        textUI = orderingBubbleUI.GetComponentInChildren<TextMeshProUGUI>();
        orderingBubbleUI.SetActive(false);
    }

    private void OnEnable()
    {
        HasOrder = false;
        startFlag = false;
        FirstOrderDone = false;
        SecondOrderDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isMove", agent.velocity.sqrMagnitude >= 0.1f);

        if (!startFlag)
        {
            startFlag = true;
            agent.enabled = true;
            agent.SetDestination(kiosk.transform.position);
            StartCoroutine(PlaceOrder());
        }
    }

    public void Init(Transform spawnPoint, GameObject kiosk, int kioskIndex)
    {
        this.spawnPoint = spawnPoint;
        this.kiosk = kiosk;
        orderInfo = GameManager.instance.GetKioskOrderInfo(kioskIndex);
    }

    // 키오스크로 이동 후 주문
    IEnumerator PlaceOrder()
    {
        yield return new WaitUntil(() => HasArrivedToDestination());
        agent.SetDestination(transform.position);
        // 키오스크 향해 회전
        while (Vector3.Angle(transform.forward, -kiosk.transform.forward) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, kiosk.transform.rotation, Time.deltaTime * -270.0f);
            yield return null;
        }

        // 도착하면 말풍선 띄우기 - 주문 중
        yield return new WaitForSeconds(0.5f);
        orderingBubbleUI.SetActive(true);
        int repeatNum = 0;
        String bubbleText = ".";
        while (repeatNum < 9)
        {
            int count = repeatNum % 3;
            bubbleText = ".";
            for (int i = 0; i < count; ++i)
            {
                bubbleText += " .";
            }
            textUI.text = bubbleText;
            repeatNum++;
            yield return new WaitForSeconds(1.0f);
        }
        orderingBubbleUI.SetActive(false);
        // 주문완료
        FirstOrderCount = UnityEngine.Random.Range(1, maxOrderCount);
        SecondOrderCount = UnityEngine.Random.Range(0, maxOrderCount);
        if(SecondOrderCount == 0)
        {
            SecondOrderDone = true;
        }
        HasOrder = true;
        orderInfo.ShowInfo(FirstOrderCount, SecondOrderCount);
    }

    public void ReceiveFirstFood(GameObject obj)
    {
        FirstOrderCount--;
        if(FirstOrderCount == 0)
        {
            FirstOrderDone = true;
        }
        stack.AddToStack(obj, eObjectType.HAMBURGER);
        orderInfo.ShowInfo(FirstOrderCount, SecondOrderCount);
    }

    public void ReceiveSecondFood(GameObject obj)
    {
        SecondOrderCount--;
        if(SecondOrderCount == 0)
        {
            SecondOrderDone = true;
        }
        stack.AddToStack(obj, eObjectType.SUBMENU);
        orderInfo.ShowInfo(FirstOrderCount, SecondOrderCount);
    }

    public void FinishOrder()
    {
        orderInfo.HideInfo();
        agent.SetDestination(spawnPoint.position);
        StartCoroutine(Exit());
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.5f); // wait for the last food to land on customer's head
        yield return new WaitUntil(() => HasArrivedToDestination());
        stack.RemoveAll();
        GameManager.instance.PoolManager.Return(gameObject);
    }

    private bool HasArrivedToDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0.0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
