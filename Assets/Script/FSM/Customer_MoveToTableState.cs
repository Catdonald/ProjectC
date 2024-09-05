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
        // 걷기 애니메이션 재생
    }
    public override void OnStateUpdate()
    {

    }
    public override void OnStateExit()
    {
        // 앉기 애니메이션 재생
        // 테이블에 들고 있던 햄버거 놓기
        customer.touchedTable.CarryingFoodCount = customer.CarryingFoodCount;
        customer.CarryingFoodCount = 0;
    }
    private CustomerController customer;
}
