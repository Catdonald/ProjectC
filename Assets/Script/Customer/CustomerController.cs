using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public GameObject entrance;
    public Transform spawnPoint;
    public Table touchedTable;
    public playerStack customerstack;
    public Line line;
    public OrderInfo orderInfo;
    public NavMeshAgent agent;

    private Animator animator;
    private LayerMask entranceLayer;
    private bool startFlag = false;

    public int OrderCount { get; set; }
    public bool HasOrder { get; set; }
    public bool ReadyToEat { get; set; }
    public int RemainOrderCount { get; set; }
    public int CarryingFoodCount { get; set; }
    public float CarryingFoodHeight { get; set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        customerstack = GetComponentInChildren<playerStack>();
    }

    private void OnEnable()
    {
        HasOrder = false;
        startFlag = false;
    }

    void Start()
    {       
            
    }

    void Update()
    {
        animator.SetBool("isMove", agent.velocity.sqrMagnitude >= 0.1f);

        if(!startFlag)
        {
            startFlag = true;
            agent.enabled = true;
            agent.SetDestination(entrance.transform.position);
            StartCoroutine(Enter());
        }
    }

    IEnumerator Enter()
    {
        yield return new WaitUntil(() => HasArrivedToDestination());

        // 도착하면 줄 n번째 자리 부여받는다.
        line.AddCustomer(this);
    }

    IEnumerator PlaceOrder()
    {
        yield return new WaitUntil(() => HasArrivedToDestination());
        // 주문할 음식 개수 결정
        DecideOrderCount();
        HasOrder = true;
        orderInfo.ShowInfo(RemainOrderCount);
    }

    IEnumerator MoveToSeat(GameObject seat)
    {
        // 마지막 음식이 다 쌓일 때까지 기다린다
        yield return new WaitForSeconds(0.3f);

        agent.SetDestination(seat.transform.position);

        yield return new WaitUntil(() => HasArrivedToDestination());

        // 테이블 방향을 바라본다
        while(Vector3.Angle(transform.forward, seat.transform.forward) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, seat.transform.rotation, Time.deltaTime * 270.0f);
            yield return null;
        }

        agent.enabled = false;
        transform.position += transform.forward * 0.3f;
        transform.position += transform.up * 0.2f;

        // 들고있던 음식 테이블에 내려놓는다
        var table = seat.GetComponentInParent<Table>();
        while(customerstack.stack.Count > 0)
        {
            table.PutFoodOnTable(customerstack.stack.Pop());
            yield return new WaitForSeconds(0.05f);
        }

        ReadyToEat = true;
        animator.SetTrigger("Eat");
    }

    IEnumerator Exit()
    {
        yield return new WaitUntil(() => HasArrivedToDestination());
        agent.SetDestination(spawnPoint.position);
        yield return new WaitUntil(() => HasArrivedToDestination());
        GameManager.instance.PoolManager.Return(gameObject);
    }

    public void UpdateQueue(Vector3 queuePointPos, bool isFirst)
    {
        agent.SetDestination(queuePointPos);

        if (isFirst)
        {
            StartCoroutine(PlaceOrder());
        }
    }

    public void AssignSeat(GameObject seat)
    {
        orderInfo.HideInfo();
        StartCoroutine(MoveToSeat(seat));
    }

    public void DecideOrderCount()
    {
        OrderCount = Random.Range(1, 5);
        RemainOrderCount = OrderCount;
    }

    public void ReceiveFood(GameObject obj,eObjectType objType , float objHeight)
    {
        CarryingFoodHeight = objHeight;
        customerstack.ReceiveObject(obj, objType, objHeight);

        RemainOrderCount -= 1;
        CarryingFoodCount += 1;

        orderInfo.ShowInfo(RemainOrderCount);

        Debug.Log("Customer : " + customerstack.stack.Count);
    }

    public void FinishEating()
    {
        agent.enabled = true;
        agent.SetDestination(entrance.transform.position);
        animator.SetTrigger("Leave");
        StartCoroutine(Exit());
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
