using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_LineWaitState : BaseState
{
    public Customer_LineWaitState(CustomerController customer)
    {
        this.customer = customer;
    }
    public override void OnStateEnter()
    {
        // ���̵� �ִϸ��̼� ���
    }
    public override void OnStateUpdate()
    {

    }
    public override void OnStateExit()
    {

    }
    private CustomerController customer;
}
