using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_EatState : BaseState
{
    private CustomerController customer;
    private float currentEatingTime = 0.0f;
    private float totalEatingTime = 5.0f;
    public Customer_EatState(CustomerController customer)
    {
        this.customer = customer;
    }
    public override void OnStateEnter()
    {
        // 먹는 애니메이션 재생

    }
    public override void OnStateUpdate()
    {
        // 먹는 동작 1번 끝날 때 테이블 위 음식 1개 사라짐
        currentEatingTime += Time.deltaTime;
        if (currentEatingTime >= totalEatingTime)
        {
            currentEatingTime = 0.0f;
            // 테이블의 자리가 모두 차지 않았을 경우
            // 테이블의 남은 음식이 1개라면 먹는 동작이 끝나도 음식 줄지 않고
            // 자리가 모두 차면 음식이 줄어든다.
            if (customer.touchedTable.CarryingFoodCount > 1)
            {
                customer.touchedTable.CarryingFoodCount--;
            }
            else
            {
                if(customer.touchedTable.IsTableFull())
                {
                    customer.touchedTable.CarryingFoodCount--;
                }
            }
            Debug.Log("Table : " + customer.touchedTable.CarryingFoodCount);
        }
    }
    public override void OnStateExit()
    {
        
    }
}
