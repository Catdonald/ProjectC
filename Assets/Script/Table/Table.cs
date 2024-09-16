using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Table : MonoBehaviour
{  

    #region Reference Properties
    public bool IsSemiFull => TrashCount == 0 && customers.Count > 0 && customers.Count < seats.Count;
    public bool IsEmpty => TrashCount == 0 && customers.Count == 0;
    public eObjectType StackType => stackType;
    #endregion

    public Receiver tableStack;
    private int TrashCount;
    [SerializeField] private eObjectType stackType;
    [SerializeField] private List<GameObject> seats;
    [SerializeField] private float baseEatTime = 5.0f;
    [SerializeField] private float baseTipChance = 0.4f;

    private List<CustomerController> customers = new List<CustomerController>();

    #region Table Stats
    private float eatTime;
    private float tipChance;
    private int tipLevel;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tableStack = GetComponentInChildren<Receiver>();

        eatTime = baseEatTime;
        tipChance = baseTipChance;
        tipLevel = 0;
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
        int trashCount = 0;
        while (tableStack.stack.Count > 0)
        {
            yield return new WaitForSeconds(eatingInterval);

            GameManager.instance.PoolManager.Return(tableStack.stack.Pop());
            trashCount++;
            LeaveTip();
        }

        while(trashCount > 0)
        {
            TrashCount++;
            trashCount--;
            yield return new WaitForSeconds(0.05f);
        }

        foreach(var customer in customers)
        {
            customer.FinishEating();
            yield return new WaitForSeconds(Random.Range(1, 4) * 0.3f);
        }

        customers.Clear();
    }

    private void LeaveTip()
    {
        if(Random.value < tipChance)
        {
            int tipAmount = Random.Range(2, 5 + tipLevel);
            for(int i = 0; i < tipAmount; i++)
            {
                // TODO) ���̺� �� ���̴� ��� AddMoney()
            }
        }
    }
}
