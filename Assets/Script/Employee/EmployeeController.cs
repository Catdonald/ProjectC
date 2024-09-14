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

        // �������� ����� ī���� ����
        var counters = GameManager.instance.counters.Where(x => x.activeInHierarchy && x.GetComponent<Counter>().GetStoredFoodCount() > 0).ToList();
        Counter counter = counters[Random.Range(0, counters.Count)].GetComponent<Counter>();

        // ī���Ϳ� �÷��̾ ������ ������ ���� �ٲ��� �ʴ´�.
        if (counter.checkWorker.IsTouchedByPlayer() ||
            counter.checkWorker.IsTouchedByStaff())
        {
            currentWork = Work.NONE;
            yield break;
        }
        // �� �ڸ��� �ϳ��� ������ ���� none
        if (!GameManager.instance.TableManager.HasAvailableSeat(counter.StackType))
        {
            currentWork = Work.NONE;
            yield break;
        }
        // ī���ͷ� ������ ����
        agent.SetDestination(counter.counterData.staffPlacePosition);

        // ī���ͷ� ���� �߿� �÷��̾ �ٸ� ������ ī���Ϳ� ����
        // ���� none���� �ٲٰ� ����
        while (!HasArrivedToDestination())
        {
            if (counter.checkWorker.IsTouchedByPlayer() ||
            (counter.checkWorker.touchedEmployee != null && counter.checkWorker.touchedEmployee != this))
            {
                // �� �ڸ��� �����
                agent.SetDestination(transform.position);
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        transform.LookAt(counter.casher.transform.position);

        // ī���Ϳ� ������ 0���� �Ǹ� ���� ����
        while (counter.GetStoredFoodCount() > 0)
        {
            // �� �ڸ��� �ϳ��� ������ ���� none
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

        // û���� ���̺��� ���ٸ� ���� none
        var trashPiles = GameManager.instance.TrashPiles.Where(x => x.Count > 0).ToList();
        if (trashPiles.Count == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }

        // û���� ���̺�� �̵�
        var trashPile = trashPiles[Random.Range(0, trashPiles.Count)];
        agent.SetDestination(trashPile.transform.position);

        /// TODO) TableŬ������ TrashObject �����ؼ� �ٲ���� �͵�..
        while (!HasArrivedToDestination())
        {
            // �̵��ϴ� ���� ���̺��� �����Ⱑ �������� ���� none
            if (trashPile.Count == 0)
            {
                agent.SetDestination(transform.position);
                currentWork = Work.NONE;
                yield break;
            }
            yield return null;
        }

        // ���̺��� ������ ��������
        while (trashPile.Count > 0)
        {
            // TODO
            //trashPile.RemoveAndStackToReceiver();
            // �ӽ��ڵ�
            trashPile.RemoveAll();
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.5f);

        // ������������ �̵�
        TrashBin trashBin = GameManager.instance.TrashBin;
        agent.SetDestination(trashBin.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // ��� �ִ� ������ ����
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

        // �� ���� ���� ī���� ã�� - receiver
        // �켱 �ܹ��� ī���͸�...
        // ī���� ��� ���� �� ���ִٸ� ���� none
        var receiver = GameManager.instance.receiver_burger;
        if (GameManager.instance.receiver_burger.IsFull)
        {
            currentWork = Work.NONE;
            yield break;
        }

        // 1�� �̻� ���� �����س��� spawner ã��(���� ī���Ϳ� ���� Ÿ���� ����)
        var validBurgerSpawners = GameManager.instance.spawners_burger.Where(x => x.Count > 0).ToList();
        // ���ٸ� ���� none
        if (validBurgerSpawners.Count == 0)
        {
            currentWork = Work.NONE;
            yield break;
        }

        // spawner �� �������� ��� ��ġ ����
        var spawner = validBurgerSpawners[Random.Range(0, validBurgerSpawners.Count)];
        agent.SetDestination(spawner.transform.position);

        // spawner�� ���� ������ ���� none
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

        // spawner���� ���� �Ѱ��� ������
        /*while(spawner.Count > 0 && Stack.Height < capacity)
        {
            spawner.RemoveAndStackObject(stack);
            yield return new WaitForSeconds(0.03f);
        }*/
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(0.5f);

        // ī���ͷ� �̵�
        agent.SetDestination(receiver.transform.position);
        yield return new WaitUntil(() => HasArrivedToDestination());

        // ī���Ϳ� ���� ����
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
