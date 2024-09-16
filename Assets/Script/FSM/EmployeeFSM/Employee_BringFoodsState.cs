using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee_BringFoodsState : BaseState
{
    private EmployeeController employee;

    public Employee_BringFoodsState(EmployeeController employee)
    {
        this.employee = employee;
    }

    public override void OnStateEnter()
    {
        // 음식기계 앞에서 2초 정도 대기
        // 대기한 후 음식을 1개 이상 가지고 있으면
        // 놓을 곳으로 움직인다.
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}
