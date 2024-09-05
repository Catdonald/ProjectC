using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_EatState : BaseState
{
    private CustomerController customer;
    private float currentEatingTime = 0.0f;
    private float totalEatingTime = 5.0f;
    public Customer_EatState(CustomerController customer)
    {
        this.customer = customer;
    }
    public override void OnStateEnter()
    {
        // �Դ� �ִϸ��̼� ���

    }
    public override void OnStateUpdate()
    {
        // �Դ� ���� 1�� ���� �� ���̺� �� ���� 1�� �����
        currentEatingTime += Time.deltaTime;
        if (currentEatingTime >= totalEatingTime)
        {
            currentEatingTime = 0.0f;
            // ���̺��� �ڸ��� ��� ���� �ʾ��� ���
            // ���̺��� ���� ������ 1����� �Դ� ������ ������ ���� ���� �ʰ�
            // �ڸ��� ��� ���� ������ �پ���.
            if (customer.touchedTable.CarryingFoodCount > 1)
            {
                customer.touchedTable.CarryingFoodCount--;
            }
            else
            {
                if(customer.touchedTable.IsTableFull())
                {
                    customer.touchedTable.CarryingFoodCount--;
                }
            }
            Debug.Log("Table : " + customer.touchedTable.CarryingFoodCount);
        }
    }
    public override void OnStateExit()
    {
        
    }
}
