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
    public eObjectType StackType => tableStack.type;
    #endregion

    public Receiver tableStack;
    public Giver trashStack;
    public int TrashCount => trashStack.Count;
    [SerializeField] private List<GameObject> seats = new List<GameObject>();

    [SerializeField] private List<CustomerController> customers = new List<CustomerController>();

    private MoneyPile moneyPile;
    private int trashCount = 0;

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
        trashStack = GetComponentInChildren<Giver>();
        moneyPile = GetComponentInChildren<MoneyPile>();
        tableStack.MaxStackCount = 50;
        trashStack.MaxStackCount = 50;
        trashObject.SetActive(false);
    }

    public override void UpgradeStats()
    {
        eatTime = (baseEatTime - (upgradeLevel - 1)) * seats.Count;
        tipChance = baseTipChance + (upgradeLevel - 1) * 0.1f;
        tipLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.Profit);
    }

    public GameObject AssignSeat(CustomerController customer)
    {
        customers.Add(customer);
        if (customers.Count >= seats.Count)
        {
            StartCoroutine(Eating());
        }
        return seats[customers.Count - 1];
    }

    public void PutFoodOnTable(GameObject food, float objHeight)
    {
        tableStack.ReceiveObject(food, StackType, objHeight);
    }

    IEnumerator Eating()
    {
        yield return new WaitUntil(() => customers.All(customer => customer.ReadyToEat));

        float eatingInterval = eatTime / tableStack.stack.Count;
        while (tableStack.stack.Count > 0)
        {
            yield return new WaitForSeconds(eatingInterval);

            GameManager.instance.PoolManager.Return(tableStack.stack.Pop());
            trashCount++;
            LeaveTip();
        }

        foreach (var customer in customers)
        {
            customer.FinishEating();
            for (int i = 0; i < trashCount; i++)
            {
                if (StackType == eObjectType.HAMBURGER)
                {
                    var trashObj = GameManager.instance.PoolManager.SpawnObject("Trash");
                    trashStack.stack.Push(trashObj);
                    trashObj.transform.position = transform.position;
                }
                else if (StackType == eObjectType.SUBMENU)
                {
                    var trashObj = GameManager.instance.PoolManager.SpawnObject("EmptyCup");
                    trashStack.stack.Push(trashObj);
                    trashObj.transform.position = transform.position;
                }
            }
            trashObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(1, 4) * 0.3f);
        }
        customers.Clear();

    }

    private void LeaveTip()
    {
        if (Random.value < tipChance)
        {
            int tipAmount = Random.Range(2, 5 + tipLevel);
            for (int i = 0; i < tipAmount; i++)
            {
                moneyPile.AddMoney();
            }
        }
    }
}
