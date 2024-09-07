using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_ExitState : BaseState
{
    private CustomerController customer;
    public Customer_ExitState(CustomerController customer)
    {
        this.customer = customer;
    }
    public override void OnStateEnter()
    {
        // 스포너로 목적지 설정
        customer.agent.destination = customer.spawner.transform.position;
        // 걷는 애니메이션 재생

    }
    public override void OnStateUpdate()
    {
        
    }
    public override void OnStateExit()
    {
        
    }
}
