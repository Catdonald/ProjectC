using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_MoveToTableState : BaseState
{
    public Customer_MoveToTableState(CustomerController customer)
    {
        this.customer = customer;
    }
    public override void OnStateEnter()
    {
        // �ȱ� �ִϸ��̼� ���
    }
    public override void OnStateUpdate()
    {

    }
    public override void OnStateExit()
    {
        // Todo) ���̺� ���� �ٶ󺸱�
        customer.PutFoodsOnTable();
        customer.touchedTable.CarryingFoodCount = customer.CarryingFoodCount;
        customer.CarryingFoodCount = 0;
    }
    private CustomerController customer;
}
