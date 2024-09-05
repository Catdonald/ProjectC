using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public GameObject counter;
    public GameObject entrance;
    public GameObject spawner;
    public GameObject lineObject;
    public GameObject sittingChair;
    public Table touchedTable;

    public NavMeshAgent agent;

    private GameObject orderUI;
    private Text orderCountText;
    private GameObject noSeatUI;
    [SerializeField]
    private State currentState;
    private BaseState enterState;
    private BaseState lineMoveState;
    private BaseState lineWaitState;
    private BaseState orderState;
    private BaseState moveToTableState;
    private BaseState eatState;
    private BaseState exitState;
    private FSM fsm;

    public int OrderCount { get; set; }
    public int CarryingFoodCount { get; set; }

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
        spawner = GameObject.Find("CustomerSpawner");
        lineObject = GameObject.Find("LineQueue");
        agent = GetComponent<NavMeshAgent>();

        currentState = State.ENTER;
        enterState = new Customer_EnterState(this);
        lineMoveState = new Customer_LineMoveState(this);
        lineWaitState = new Customer_LineWaitState(this);
        orderState = new Customer_OrderState(this);
        moveToTableState = new Customer_MoveToTableState(this);
        eatState = new Customer_EatState(this);
        exitState = new Customer_ExitState(this);
        fsm = new FSM(enterState);
    }

    // Start is called before the first frame update
    void Start()
    {
        orderUI = transform.GetChild(1).GetChild(0).gameObject;
        noSeatUI = transform.GetChild(1).GetChild(1).gameObject;
        orderCountText = orderUI.GetComponentInChildren<Text>();

        orderUI.SetActive(false);
        noSeatUI.SetActive(false);

        // 목적지 설정할 때 NavMeshAgent 컴포넌트 비활성화 했다가 활성화 해줘야 제대로 설정된다..
        agent.enabled = false;
        agent.enabled = true;
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
                    // agent 목적지 설정
                    agent.destination = lineObject.transform.position;
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
                    orderUI.SetActive(false);
                    // 빈 자리가 있다면 그곳으로 이동
                    sittingChair = GameManager.instance.TableManager.GetEmptySeat();
                    if (sittingChair != null)
                    {
                        ChangeState(State.MOVETOTABLE);
                        noSeatUI.SetActive(false);
                        // 목적지 설정
                        agent.destination = sittingChair.transform.position;
                    }
                    else
                    {
                        noSeatUI.SetActive(true);
                    }
                }
                break;
            case State.MOVETOTABLE:
                if (CheckAgentReachToDestination())
                {
                    sittingChair.GetComponent<Chair>().SetSittingCustomer(this);
                    ChangeState(State.EAT);
                }
                break;
            case State.EAT:
                if (touchedTable.CarryingFoodCount == 0)
                {
                    ChangeState(State.EXIT);
                }
                break;
            case State.EXIT:
                if (CheckAgentReachToDestination())
                {
                    GameManager.instance.PoolManager.Return(gameObject);
                }
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
            case State.EAT:
                fsm.ChangeState(eatState);
                break;
            case State.EXIT:
                fsm.ChangeState(exitState);
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
            Debug.Log("Collide with Entrance!");
        }
        else if (other.gameObject.CompareTag("Order"))
        {
            isCollideWithCounter = true;
        }
        else if (other.gameObject.CompareTag("Table"))
        {
            touchedTable = other.transform.parent.gameObject.GetComponent<Table>();
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
        else if (other.gameObject.CompareTag("Table"))
        {
            touchedTable = null;
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

    public void ReceiveFood(int count)
    {
        OrderCount -= count;
        CarryingFoodCount += count;
        orderCountText.text = OrderCount.ToString();
        Debug.Log("Customer : " + CarryingFoodCount);
    }

    public void PutFoodsOnTable()
    {

    }
}
