using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_OrderState : BaseState
{
    public Customer_OrderState(CustomerController customer)
    {
        this.customer = customer;
    }
    public override void OnStateEnter()
    {
        // ���̵� �ִϸ��̼� ���
        // ������ ������ ��� �ִ� ���̵� �ִϸ��̼� ���
        // �����ֹ� UI ����
        customer.SetOrderCountText(customer.OrderCount);
        customer.SetActiveOrderUI(true);
    }
    public override void OnStateUpdate()
    {
        
    }
    public override void OnStateExit()
    {
        
    }
    private CustomerController customer;
}
