using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.PlayerSettings;
#endif
public class EmployeeController : MonoBehaviour
{
    public playerStack Stack => stack;

    [SerializeField] private float baseSpeed = 2.5f;
    [SerializeField] private int baseCapacity = 3;

    private Animator animator;
    private NavMeshAgent agent;
    private LayerMask entranceLayer;
    private Work currentWork;
    private playerStack stack;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        entranceLayer = 1 << LayerMask.NameToLayer("Entrance");
        stack = GetComponentInChildren<playerStack>();
        currentWork = Work.NONE;
        GameManager.instance.OnUpgrade += UpdateStats;
        UpdateStats();
        StartCoroutine(CheckDoor());
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
                StartCoroutine(RefillBurgerToCounter());
                break;
            case 3:
                StartCoroutine(RefillBurgerToPackageTable());
                break;
            case 4:
                StartCoroutine(RefillPackage());
                break;
            case 5:
                StartCoroutine(RefillSubMenu());
                break;
            case 6:
                StartCoroutine(PackingBurgerPack());
                break;
            case 7:
                StartCoroutine(TakeDriveThruOrder());
                break;
            default:
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
        stack.Capacity = baseCapacity + capacityLevel;
    }

    IEnumerator CheckDoor()
    {
        RaycastHit hit;
        while (!Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 0.5f, entranceLayer, QueryTriggerInteraction.Collide))
        {
            yield return null;
        }

        var doors = hit.transform.GetComponentsInChildren<Door>();
        foreach (var door in doors)
        {
            door.OpenDoor(transform);
        }

        yield return new WaitForSeconds(2.0f);

        foreach (var door in doors)
        {
            door.CloseDoor();
        }
    }

    IEnumerator TakeOrder()
    {
        currentWork = Work.TAKEORDER;

        var counters = GameManager.instance.counters.Where(x => x.gameObject.activeInHierarchy && x.GetStoredFoodCount > 0).ToList();
        if (counters.Count == 0)
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
        while (counter.GetStoredFoodCount > 0)
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

        // 카운터에 다른 직원이나 플레이어가 있거나 음식 없으면 상태 none
        if (dtCounters.HasWorker || dtCounters.GetStoredFoodCount == 0)
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
        while (dtCounters.GetStoredFoodCount > 0)
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

        while (dirtyTable.trashStack.Count > 0)
        {
            dirtyTable.trashStack.RemoveAndStackObject(stack);
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.5f);

        // 쓰레기통으로 이동
        var trashBins = GameManager.instance.trashBins;
        Trashbin trashBin = trashBins[0];
        float minDistance = float.MaxValue;
        foreach (var bin in trashBins)
        {
           float distance = Vector3.Distance(transform.position, bin.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                trashBin = bin;
            }
        }
        agent.SetDestination(trashBin.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // TODO) 쓰레기통에 쓰레기 한개씩 넣기
        while (stack.Count > 0)
        {
            trashBin.ThrowToBin(stack);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.5f);
        currentWork = Work.NONE;
    }

    IEnumerator RefillBurgerToCounter()
    {
        currentWork = Work.REFILLBURGERTOCOUNTER;

        var counter = GameManager.instance.counters.Where(x => x.StackType == eObjectType.HAMBURGER && x.gameObject.activeInHierarchy).FirstOrDefault();
        if (!counter || counter.IsStorageFull)
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

        // take foods from the spawner
        while (spawner.Count > 0 && stack.Height < stack.Capacity)
        {
            var food = spawner.RequestObject();
            if (food != null)
            {
                Stack.AddToStack(food, spawner.type);
            }
            yield return new WaitForSeconds(0.03f);
        }
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(0.5f);

        agent.SetDestination(counter.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // put foods on the counter
        while (stack.Count > 0)
        {
            if (!counter.IsStorageFull)
            {
                eObjectType type = stack.StackType;
                var food = stack.RemoveFromStack();
                counter.Receiver.ReceiveObject(food, type, GameManager.instance.GetStackOffset(type));
            }
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.5f);
        currentWork = Work.NONE;
    }

    IEnumerator RefillBurgerToPackageTable()
    {
        currentWork = Work.REFILLBURGERTOPACKAGETABLE;

        var packageTable = GameManager.instance.PackageTable;
        if (!packageTable.gameObject.activeInHierarchy || packageTable.IsFoodStorageFull)
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

        // take foods from the spawner
        while (spawner.Count > 0 && stack.Height < stack.Capacity)
        {
            var food = spawner.RequestObject();
            if (food != null)
            {
                Stack.AddToStack(food, spawner.type);
            }
            yield return new WaitForSeconds(0.03f);
        }
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(0.5f);

        if(packageTable.UpgradeLevel > 1)
        {
            agent.SetDestination(packageTable.BurgerStoragePosition);
        }
        else
        {
            agent.SetDestination(packageTable.WorkingSpotPosition);
        }       
        yield return new WaitUntil(() => HasArrivedToDestination());

        // put foods on the counter
        while (stack.Count > 0)
        {
            if (!packageTable.IsFoodStorageFull)
            {
                eObjectType type = stack.StackType;
                var food = stack.RemoveFromStack();
                packageTable.foodReceiver.ReceiveObject(food, type, GameManager.instance.GetStackOffset(type));
            }
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.5f);
        currentWork = Work.NONE;
    }

    IEnumerator RefillPackage()
    {
        currentWork = Work.REFILLPACKAGE;
        // dtCounter에 package 가득 차있다면 상태 none
        var dtCounter = GameManager.instance.DriveThruCounter;
        if (!dtCounter.gameObject.activeInHierarchy || dtCounter.IsStorageFull)
        {
            currentWork = Work.NONE;
            yield break;
        }

        // packageTable에 package 한개도 없으면 상태 none
        var packageTable = GameManager.instance.PackageTable;
        if (packageTable.GetStoredPackageCount == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }

        agent.SetDestination(packageTable.PackageStoragePosition);

        // 가는 도중 packageTable에에 패키지 한개도 없으면 상태 none
        while (!HasArrivedToDestination())
        {
            if (packageTable.GetStoredPackageCount == 0)
            {
                agent.SetDestination(transform.position);
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        // take packages from the packageTable
        while (packageTable.GetStoredPackageCount > 0 && stack.Height < stack.Capacity)
        {
            var package = packageTable.packageStack.RequestObject();
            if (package != null)
            {
                stack.AddToStack(package, packageTable.packageStack.type);
            }
            yield return new WaitForSeconds(0.03f);
        }
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(0.5f);

        agent.SetDestination(dtCounter.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // put foods on the counter
        while (stack.Count > 0)
        {
            if (!dtCounter.IsStorageFull)
            {
                eObjectType type = stack.StackType;
                var package = stack.RemoveFromStack();
                dtCounter.Receiver.ReceiveObject(package, type, GameManager.instance.GetStackOffset(type));
            }
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.5f);
        currentWork = Work.NONE;
    }

    IEnumerator RefillSubMenu()
    {
        currentWork = Work.REFILLSUBMENU;

        var counter = GameManager.instance.counters.Where(x => x.StackType == eObjectType.SUBMENU && x.gameObject.activeInHierarchy).FirstOrDefault();
        if (!counter || counter.IsStorageFull)
        {
            currentWork = Work.NONE;
            yield break;
        }

        var validSubmenuSpawners = GameManager.instance.spawners_subMenu.Where(x => x.Count > 0).ToList();
        if (validSubmenuSpawners.Count == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }

        var spawner = validSubmenuSpawners[Random.Range(0, validSubmenuSpawners.Count)];
        agent.SetDestination(spawner.transform.position);

        // 가는 도중 spawner에 음식 한개도 없으면 상태 none
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

        // take foods from the spawner
        while (spawner.Count > 0 && stack.Height < stack.Capacity)
        {
            var food = spawner.RequestObject();
            if (food != null)
            {
                Stack.AddToStack(food, spawner.type);
            }
            yield return new WaitForSeconds(0.03f);
        }
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(0.5f);

        agent.SetDestination(counter.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // put foods on the counter
        while (stack.Count > 0)
        {
            if (!counter.IsStorageFull)
            {
                eObjectType type = stack.StackType;
                var food = stack.RemoveFromStack();
                counter.Receiver.ReceiveObject(food, type, GameManager.instance.GetStackOffset(type));
            }
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.5f);
        currentWork = Work.NONE;
    }

    IEnumerator PackingBurgerPack()
    {
        currentWork = Work.PACKING;

        var packageTable = GameManager.instance.PackageTable;
        if (packageTable.HasWorker || packageTable.IsPackageStorageFull || packageTable.GetStoredFoodCount == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }

        agent.SetDestination(packageTable.WorkingSpotPosition);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // 테이블 향해 회전
        transform.LookAt(packageTable.transform.position);

        while (packageTable.GetStoredFoodCount > 0)
        {
            if(packageTable.HasWorker && packageTable.WorkingEmployee != this)
            {
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        currentWork = Work.NONE;
    }

    public enum Work
    {
        NONE,
        TAKEORDER,
        TAKEDTORDER,
        PACKING,
        CLEANUP,
        REFILLBURGERTOCOUNTER,
        REFILLBURGERTOPACKAGETABLE,
        REFILLPACKAGE,
        REFILLSUBMENU,
        MAX
    }
}
