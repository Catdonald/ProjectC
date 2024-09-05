using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_ExitState : BaseState
{
    public Customer_ExitState(CustomerController customer)
    {
        this.customer = customer;
    }
    public override void OnStateEnter()
    {
        // �����ʷ� ������ ����
        customer.agent.destination = customer.spawner.transform.position;
        // �ȴ� �ִϸ��̼� ���

    }
    public override void OnStateUpdate()
    {
        
    }
    public override void OnStateExit()
    {
        
    }
    private CustomerController customer;
}
