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
        // 걷기 애니메이션 재생
        // 목적지 설정
        // 음식과 개수 정하기
        // 목적지 설정할 때 NavMeshAgent 컴포넌트 비활성화 했다가 활성화 해줘야 제대로 설정된다..
        customer.agent.enabled = false;
        customer.agent.enabled = true;
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
