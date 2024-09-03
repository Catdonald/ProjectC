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
        // 아이들 애니메이션 재생
    }
    public override void OnStateUpdate()
    {

    }
    public override void OnStateExit()
    {

    }
    private CustomerController customer;
}
