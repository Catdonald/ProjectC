using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class EmployeeController : MonoBehaviour
{
    public playerStack Stack => stack;

    [SerializeField] private float baseSpeed = 2.5f;
    [SerializeField] private int baseCapacity = 3;

    private Animator animator;
    private NavMeshAgent agent;
    private Work currentWork;
    private playerStack stack;
    private int capacity;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentWork = Work.NONE;
        GameManager.instance.OnUpgrade += UpdateStats;
        UpdateStats();
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
            case 5:
                StartCoroutine(TakeDriveThruOrder());
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

    private void UpdateStats()
    {
        int speedLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.EmployeeSpeed);
        agent.speed = baseSpeed + (speedLevel * 0.1f);

        int capacityLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.EmployeeCapacity);
        capacity = baseCapacity + capacityLevel;
    }

    IEnumerator TakeOrder()
    {
        currentWork = Work.TAKEORDER;

        var counters = GameManager.instance.counters.Where(x => x.gameObject.activeInHierarchy && x.GetStoredFoodCount() > 0).ToList();
        if(counters.Count == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }
        Counter counter = counters[Random.Range(0, counters.Count)].GetComponent<Counter>();

        // 카운터에 다른 직원이나 플레이어가 있으면 상태 none
        if (counter.HasWorker)
        {
            currentWork = Work.NONE;
            yield break;
        }
        // 자리남은 테이블이 없으면 상태 none
        if (!TableManager.Instance.HasAvailableSeat(counter.StackType))
        {
            currentWork = Work.NONE;
            yield break;
        }
        agent.SetDestination(counter.WorkingSpotPosition);

        // 카운터로 이동하는 중에 카운터에 플레이어나 다른 직원이 서면 상태 none
        while (!HasArrivedToDestination())
        {
            if (counter.HasWorker && counter.WorkingEmployee != this)
            {
                agent.SetDestination(transform.position);
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        transform.LookAt(counter.casher.transform.position);

        // 카운터에 음식 남아있는 동안
        while (counter.GetStoredFoodCount() > 0)
        {
            if (!TableManager.Instance.HasAvailableSeat(counter.StackType))
            {
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        currentWork = Work.NONE;
    }

    IEnumerator TakeDriveThruOrder()
    {
        currentWork = Work.TAKEDTORDER;

        var dtCounters = GameManager.instance.DriveThruCounter;
        if (!dtCounters.gameObject.activeInHierarchy)
        {
            currentWork = Work.NONE;
            yield break;
        }

        // 카운터에 다른 직원이나 플레이어가 있으면 상태 none
        if (dtCounters.HasWorker)
        {
            currentWork = Work.NONE;
            yield break;
        }
        
        agent.SetDestination(dtCounters.WorkingSpotPosition);

        // 카운터로 이동하는 중에 카운터에 플레이어나 다른 직원이 서면 상태 none
        while (!HasArrivedToDestination())
        {
            if (dtCounters.HasWorker && dtCounters.WorkingEmployee != this)
            {
                agent.SetDestination(transform.position);
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        transform.LookAt(dtCounters.casher.transform.position);

        // 카운터에 음식 남아있는 동안
        while (dtCounters.GetStoredFoodCount() > 0)
        {
            yield return null;
        }

        currentWork = Work.NONE;
    }

    IEnumerator CleanTable()
    {
        currentWork = Work.CLEANUP;

        // 더러워진 테이블이 없다면 상태 none
        var dirtyTables = TableManager.Instance.DirtyTables;
        if (dirtyTables.Count == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }

        var dirtyTable = dirtyTables[Random.Range(0, dirtyTables.Count)];
        agent.SetDestination(dirtyTable.transform.position);

        /// TODO) TableŬ������ TrashObject �����ؼ� �ٲ���� �͵�..
        while (!HasArrivedToDestination())
        {
            // 가는 중에 테이블이 청소되면 상태 none
            if (dirtyTable.TrashCount == 0)
            {
                agent.SetDestination(transform.position);
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        while (dirtyTable.TrashCount > 0)
        {
            dirtyTable.trashStack.RemoveAndStackObject(stack);
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.5f);

        // 쓰레기통으로 이동
        Trashbin trashBin = GameManager.instance.TrashBin;
        agent.SetDestination(trashBin.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // TODO) 쓰레기통에 쓰레기 한개씩 넣기
        while(stack.Count > 0)
        {
            trashBin.ThrowToBin(stack);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.5f);
        currentWork = Work.NONE;
    }

    IEnumerator RefillFood()
    {
        currentWork = Work.REFILLFOOD;

        // TODO
        // 랜덤으로 숫자 한개 뽑아서 그 숫자에 맞는 카운터 찾기
        // 우선은 버거 카운터만..
        var counter = GameManager.instance.counters.Where(x => x.StackType == eObjectType.HAMBURGER).First();
        if (counter.IsFoodStorageFull())
        {
            currentWork = Work.NONE;
            yield break;
        }

        var validBurgerSpawners = GameManager.instance.spawners_burger.Where(x => x.Count > 0).ToList();
        if (validBurgerSpawners.Count == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }

        var spawner = validBurgerSpawners[Random.Range(0, validBurgerSpawners.Count)];
        agent.SetDestination(spawner.transform.position);

        // 가는 도중 spawner에 햄버거 한개도 없으면 상태 none
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

        // TODO take foods from the spawner
        /*while(spawner.Count > 0 && Stack.Height < capacity)
        {
            spawner.RemoveAndStackObject(stack);
            yield return new WaitForSeconds(0.03f);
        }*/
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(0.5f);

        agent.SetDestination(counter.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // TODO put foods on the counter
        /*while(stack.Count > 0)
        {
            if(!counter.IsFoodStorageFull())
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
    public enum Work
    {
        NONE,
        TAKEORDER,
        TAKEDTORDER,
        PACKING,
        CLEANUP,
        REFILLFOOD,
        REFILLPACKAGE,
        MAX
    }
}
