using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Table : Upgradable
{
    [SerializeField] private GameObject trashObject;

    #region Reference Properties
    public bool IsSemiFull => TrashCount == 0 && customers.Count > 0 && customers.Count < seats.Count;
    public bool IsEmpty => TrashCount == 0 && customers.Count == 0;
    public eObjectType StackType => stackType;
    #endregion

    public Receiver tableStack;
    public int TrashCount { get; set; }
    [SerializeField] private eObjectType stackType;
    [SerializeField] private List<GameObject> seats = new List<GameObject>();

    private List<CustomerController> customers = new List<CustomerController>();

    #region Table Stats
    [SerializeField, Range(1.0f, 10.0f)] private float baseEatTime = 5.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float baseTipChance = 0.4f;
    private float eatTime;
    private float tipChance;
    private int tipLevel;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tableStack = GetComponentInChildren<Receiver>();
        trashObject.SetActive(false);
    }

    protected override void UpgradeStats()
    {
        eatTime = (baseEatTime - (upgradeLevel - 1)) * seats.Count;
        tipChance = baseTipChance + (upgradeLevel - 1) * 0.1f;
        // TODO) tipLevel
    }

    public GameObject AssignSeat(CustomerController customer)
    {
        customers.Add(customer);
        if(customers.Count >= seats.Count)
        {
            StartCoroutine(Eating());
        }
        return seats[customers.Count - 1];
    }

    public void PutFoodOnTable(GameObject food)
    {
        tableStack.stack.Push(food);
    }

    IEnumerator Eating()
    {
        yield return new WaitUntil(() => customers.All(customer => customer.ReadyToEat));

        float eatingInterval = eatTime / tableStack.stack.Count;
        while (tableStack.stack.Count > 0)
        {
            yield return new WaitForSeconds(eatingInterval);

            GameManager.instance.PoolManager.Return(tableStack.stack.Pop());
            TrashCount++;
            LeaveTip();
        }

        foreach(var customer in customers)
        {
            customer.FinishEating();
            yield return new WaitForSeconds(Random.Range(1, 4) * 0.3f);
        }
        customers.Clear();

        trashObject.SetActive(true);
    }

    private void LeaveTip()
    {
        if(Random.value < tipChance)
        {
            int tipAmount = Random.Range(2, 5 + tipLevel);
            for(int i = 0; i < tipAmount; i++)
            {
                // TODO) AddMoney()
            }
        }
    }

    // 임시
    public void Clean()
    {
        TrashCount = 0;
        trashObject.SetActive(false);
    }
}
