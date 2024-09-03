using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public GameObject counter;
    public GameObject entrance;
    public NavMeshAgent agent;

    private GameObject orderUI;
    private Text orderCountText;
    private GameObject noSeatUI;

    private State currentState;
    private BaseState enterState;
    private BaseState lineMoveState;
    private BaseState lineWaitState;
    private BaseState orderState;
    private BaseState moveToTableState;
    private FSM fsm;

    public int OrderCount { get; set; }

    private bool isCollideWithEntrance = false;
    private bool isCollideWithCounter = false;

    public enum State
    {
        ENTER,
        LINEMOVE,
        LINEWAIT,
        ORDER,
        MOVETOTABLE,
        EAT,
        EXIT,
        MAX
    }

    private void Awake()
    {
        counter = GameObject.Find("InteractionRange_Customer");
        entrance = GameObject.Find("Entrance");

        currentState = State.ENTER;
        enterState = new Customer_EnterState(this);
        lineMoveState = new Customer_LineMoveState(this);
        lineWaitState = new Customer_LineWaitState(this);
        orderState = new Customer_OrderState(this);
        moveToTableState = new Customer_MoveToTableState(this);
        fsm = new FSM(enterState);
    }

    // Start is called before the first frame update
    void Start()
    {
        orderUI = transform.GetChild(0).GetChild(0).gameObject;
        noSeatUI = transform.GetChild(0).GetChild(1).gameObject;
        orderCountText = orderUI.GetComponentInChildren<Text>();
        agent = GetComponent<NavMeshAgent>();

        orderUI.SetActive(false);
        noSeatUI.SetActive(false);
        agent.destination = entrance.transform.position;
        DecideFoodAndCount();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.ENTER:
                if (isCollideWithEntrance)
                {
                    ChangeState(State.LINEMOVE);
                }
                break;
            case State.LINEMOVE:
                if (isCollideWithCounter)
                {
                    if (CheckAgentReachToDestination())
                    {
                        ChangeState(State.ORDER);
                    }
                }
                else
                {
                    if (CheckAgentReachToDestination())
                    {
                        ChangeState(State.LINEWAIT);
                    }
                }
                break;
            case State.LINEWAIT:
                if (agent.velocity.sqrMagnitude > 0.0f)
                {
                    ChangeState(State.LINEMOVE);
                }
                break;
            case State.ORDER:
                if (OrderCount == 0)
                {
                    // 빈 테이블이 있다면
                    // ChangeState(State.MOVETOTABLE);
                }
                break;
            case State.MOVETOTABLE:
                break;
        }
        fsm.UpdateState();
    }
    private void ChangeState(State nextState)
    {
        currentState = nextState;
        switch (currentState)
        {
            case State.ENTER:
                fsm.ChangeState(enterState);
                break;
            case State.LINEMOVE:
                fsm.ChangeState(lineMoveState);
                break;
            case State.LINEWAIT:
                fsm.ChangeState(lineWaitState);
                break; 
            case State.ORDER:
                fsm.ChangeState(orderState);
                break;
            case State.MOVETOTABLE:
                fsm.ChangeState(moveToTableState);
                break;
        }
    }
    private void OnEnable()
    {
        ChangeState(State.ENTER);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Entrance"))
        {
            isCollideWithEntrance = true;
        }
        else if (other.gameObject.CompareTag("Order"))
        {
            isCollideWithCounter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Entrance"))
        {
            isCollideWithEntrance = false;
        }
        else if (other.gameObject.CompareTag("Order"))
        {
            isCollideWithCounter = false;
        }
    }

    private bool CheckAgentReachToDestination()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (agent.velocity.sqrMagnitude == 0.0f)
            {
                return true;
            }
        }
        return false;
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public void SetAgentDestination(Vector3 position)
    {
        agent.destination = position;
    }

    public void DecideFoodAndCount()
    {
        // 1 이상 4 이하의 랜덤한 정수
        OrderCount = Random.Range(1, 5);
    }

    public void SetActiveOrderUI(bool active)
    {
        orderUI.SetActive(active);
    }
    public void SetActiveNoSeatUI(bool active)
    {
        noSeatUI.SetActive(active);
    }
    public void SetOrderCountText(int count)
    {
        orderCountText.text = count.ToString();
    }
}
