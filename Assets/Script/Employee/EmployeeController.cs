using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class EmployeeController : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;

    [SerializeField]
    private Work currentWork;

    public enum Work
    {
        NONE,
        TAKEORDER,
        PACKING,
        CLEANUP,
        REFILLFOOD,
        REFILLPACKAGE,
        MAX
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentWork = Work.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isMove", agent.velocity.sqrMagnitude > 0.1f);
        HandleWork();
    }

    private void HandleWork()
    {
        if (currentWork != Work.NONE)
        {
            return;
        }
        
        switch (Random.Range(0, ((int)Work.MAX) - 1))
        {
            case 0:
                StartCoroutine(TakeOrder());
                break;
            case 1:
                StartCoroutine(CleanTable());
                break;
            case 2:
                StartCoroutine(RefillFood());
                break;
            case 3:
                StartCoroutine(RefillPackage());
                break;
            case 4:
                StartCoroutine(PackingBurgerPack());
                break;
        }
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

    IEnumerator TakeOrder()
    {
        currentWork = Work.TAKEORDER;

        // 랜덤으로 담당할 카운터 선정
        var counters = GameManager.instance.counters.Where(x => x.activeInHierarchy && x.GetComponent<Counter>().GetStoredFoodCount() > 0).ToList();
        Counter counter = counters[Random.Range(0, counters.Count)].GetComponent<Counter>();

        // 카운터에 플레이어나 직원이 있으면 상태 바꾸지 않는다.
        if (counter.checkWorker.IsTouchedByPlayer() ||
            counter.checkWorker.IsTouchedByStaff())
        {
            currentWork = Work.NONE;
            yield break;
        }
        // 빈 자리가 하나도 없으면 상태 none
        if (!GameManager.instance.TableManager.HasAvailableSeat(counter.StackType))
        {
            currentWork = Work.NONE;
            yield break;
        }
        // 카운터로 목적지 설정
        agent.SetDestination(counter.counterData.staffPlacePosition);

        // 카운터로 가는 중에 플레이어나 다른 직원이 카운터에 서면
        // 상태 none으로 바꾸고 종료
        while (!HasArrivedToDestination())
        {
            if (counter.checkWorker.IsTouchedByPlayer() ||
            (counter.checkWorker.touchedEmployee != null && counter.checkWorker.touchedEmployee != this))
            {
                // 그 자리에 멈춘다
                agent.SetDestination(transform.position);
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        transform.LookAt(counter.casher.transform.position);

        // 카운터에 음식이 0개가 되면 상태 종료
        while (counter.GetStoredFoodCount() > 0)
        {
            // 빈 자리가 하나도 없으면 상태 none
            if (!GameManager.instance.TableManager.HasAvailableSeat(counter.StackType))
            {
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        currentWork = Work.NONE;
    }

    IEnumerator CleanTable()
    {
        currentWork = Work.CLEANUP;

        // 청소할 테이블이 없다면 상태 none
        var trashPiles = GameManager.instance.TrashPiles.Where(x => x.Count > 0).ToList();
        if (trashPiles.Count == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }

        // 청소할 테이블로 이동
        var trashPile = trashPiles[Random.Range(0, trashPiles.Count)];
        agent.SetDestination(trashPile.transform.position);

        /// TODO) Table클래스의 TrashObject 관련해서 바꿔야할 것들..
        while (!HasArrivedToDestination())
        {
            // 이동하는 동안 테이블의 쓰레기가 없어지면 상태 none
            if (trashPile.Count == 0)
            {
                agent.SetDestination(transform.position);
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        // 테이블의 쓰레기 가져오기
        while (trashPile.Count > 0)
        {
            // TODO
            //trashPile.RemoveAndStackToReceiver();
            // 임시코드
            trashPile.RemoveAll();
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.5f);

        // 쓰레기통으로 이동
        TrashBin trashBin = GameManager.instance.TrashBin;
        agent.SetDestination(trashBin.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // 들고 있는 쓰레기 비우기
        /*while(stack.Count > 0)
        {
            trashBin.ThrowToBin(stack);
            yield return new WaitForSeconds(0.03f);
        }*/
        yield return new WaitForSeconds(0.5f);
        currentWork = Work.NONE;
    }

    IEnumerator RefillFood()
    {
        currentWork = Work.REFILLFOOD;

        // 다 차지 않은 카운터 찾기 - receiver
        // 우선 햄버거 카운터만...
        // 카운터 모두 음식 다 차있다면 상태 none
        var receiver = GameManager.instance.receiver_burger;
        if (GameManager.instance.receiver_burger.IsFull)
        {
            currentWork = Work.NONE;
            yield break;
        }

        // 1개 이상 음식 생성해놓은 spawner 찾기(위의 카운터와 음식 타입이 같은)
        var validBurgerSpawners = GameManager.instance.spawners_burger.Where(x => x.Count > 0).ToList();
        // 없다면 상태 none
        if (validBurgerSpawners.Count == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }

        // spawner 중 랜덤으로 골라서 위치 지정
        var spawner = validBurgerSpawners[Random.Range(0, validBurgerSpawners.Count)];
        agent.SetDestination(spawner.transform.position);

        // spawner에 음식 없으면 상태 none
        while (!HasArrivedToDestination())
        {
            if (spawner.Count == 0)
            {
                agent.SetDestination(transform.position);
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        // spawner에서 음식 한개씩 가져옴
        /*while(spawner.Count > 0 && Stack.Height < capacity)
        {
            spawner.RemoveAndStackObject(stack);
            yield return new WaitForSeconds(0.03f);
        }*/
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(0.5f);

        // 카운터로 이동
        agent.SetDestination(receiver.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // 카운터에 음식 놓기
        /*while(stack.Count > 0)
        {
            if(!receiver.IsFull)
            {
                var food = stack.RemoveFromStack();
                receiver.AddToStack(food);
            }
            yield return new WaitForSeconds(0.03f);
        }*/
        yield return new WaitForSeconds(0.5f);
        currentWork = Work.NONE;
    }

    IEnumerator RefillPackage()
    {
        currentWork = Work.NONE;
        yield break;
    }

    IEnumerator PackingBurgerPack()
    {
        currentWork = Work.NONE;
        yield break;
    }
}
