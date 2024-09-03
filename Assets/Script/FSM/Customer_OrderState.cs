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
        customer.SetActiveOrderUI(true);
    }
    public override void OnStateUpdate()
    {
        // ������ ������ �����ֹ� UI�� �����ؽ�Ʈ ����
        // ���� ��� ������ �����ֹ� UI ����
        if (customer.OrderCount == 0)
        {
            customer.SetActiveOrderUI(false);
            // �ڸ��� ���ٸ�
            customer.SetActiveNoSeatUI(true);
        }
        customer.SetOrderCountText(customer.OrderCount);
    }
    public override void OnStateExit()
    {
        // NoSeat UI ����
        customer.SetActiveNoSeatUI(false);
    }
    private CustomerController customer;
}
