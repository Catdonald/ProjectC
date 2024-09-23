using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private int maxOrderCount = 5;

    private Transform entrance;
    private Transform spawnPoint;
    private Receiver customerstack;
    private Counter counter;
    private OrderInfo orderInfo;
    private NavMeshAgent agent;
    private Animator animator;
    private LayerMask entranceLayer;
    private bool startFlag = false;

    public int OrderCount { get; set; }
    public bool HasOrder { get; set; }
    public bool ReadyToEat { get; set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        customerstack = GetComponentInChildren<Receiver>();
    }

    private void OnEnable()
    {
        HasOrder = false;
        startFlag = false;
    }

    void Update()
    {
        animator.SetBool("isMove", agent.velocity.sqrMagnitude >= 0.1f);

        if(!startFlag)
        {
            startFlag = true;
            agent.enabled = true;
            agent.SetDestination(entrance.position);
            StartCoroutine(Enter());
        }
    }

    public void Init(Transform spawnPoint, Transform entrance, Counter counter, OrderInfo orderInfo)
    {
        this.spawnPoint = spawnPoint;
        this.entrance = entrance;
        this.counter = counter;
        this.orderInfo = orderInfo;
    }

    IEnumerator Enter()
    {
        yield return new WaitUntil(() => HasArrivedToDestination());

        // 도착하면 줄 n번째 자리 부여받는다.
        counter.AddCustomer(this);
    }

    IEnumerator PlaceOrder()
    {
        yield return new WaitUntil(() => HasArrivedToDestination());
        OrderCount = Random.Range(1, maxOrderCount);
        HasOrder = true;
        orderInfo.ShowInfo(OrderCount);
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
            table.PutFoodOnTable(customerstack.stack.Pop(), customerstack.objectHeight);
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

    public void ReceiveFood(GameObject obj,eObjectType objType , float objHeight)
    {
        customerstack.ReceiveObject(obj, objType, objHeight);
        OrderCount -= 1;
        orderInfo.ShowInfo(OrderCount);

        Debug.Log("Customer : " + customerstack.stack.Count);
    }

    public void FinishEating()
    {
        agent.enabled = true;
        agent.SetDestination(entrance.position);
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
