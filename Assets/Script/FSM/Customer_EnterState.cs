using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_EnterState : BaseState
{
    public Customer_EnterState(CustomerController customer)
    {
        this.customer = customer;
    }
    public override void OnStateEnter()
    {
        // �ȱ� �ִϸ��̼� ���
        // ������ ����
        // ���İ� ���� ���ϱ�
        customer.agent.destination = customer.entrance.transform.position;
        customer.DecideFoodAndCount();
    }
    public override void OnStateUpdate()
    {
        
    }
    public override void OnStateExit()
    {
        
    }

    private CustomerController customer;
}
