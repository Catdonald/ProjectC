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
        // 아이들 애니메이션 재생
        // 음식을 받으면 들고 있는 아이들 애니메이션 재생
        // 음식주문 UI 띄우기
        customer.SetActiveOrderUI(true);
    }
    public override void OnStateUpdate()
    {
        // 음식을 받으면 음식주문 UI의 숫자텍스트 변경
        customer.SetOrderCountText(customer.OrderCount);
    }
    public override void OnStateExit()
    {
        
    }
    private CustomerController customer;
}
