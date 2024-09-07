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
        customer.transform.LookAt(customer.touchedTable.transform.position);
        //totalEatingTime = customer.touchedTable.tableData.eatSpeed;
        // �Դ� �ִϸ��̼� ���

    }
    public override void OnStateUpdate()
    {
        // totalEatingTime ������ ���̺� �� ���� 1�� �����
        currentEatingTime += Time.deltaTime;
        if (currentEatingTime >= totalEatingTime)
        {
            currentEatingTime = 0.0f;
            // ���̺��� �ڸ��� ��� ���� �ʾ��� ���
            // ���̺��� ���� ������ 1����� �Դ� ������ ������ ���� ���� �ʰ�
            // �ڸ��� ��� ���� ������ �پ���.
            if (customer.touchedTable.CarryingFoodCount > 1)
            {
                customer.touchedTable.RemoveFoodOnTable();
            }
            else
            {
                if(customer.touchedTable.IsTableFull())
                {
                    customer.touchedTable.RemoveFoodOnTable();
                }
            }
            Debug.Log("Table : " + customer.touchedTable.CarryingFoodCount);
        }
    }
    public override void OnStateExit()
    {
        // Table ������ ������Ʈ Ȱ��ȭ
        customer.touchedTable.MakeDirtyTable();
    }
}
