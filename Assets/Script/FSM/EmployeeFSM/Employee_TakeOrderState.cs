using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee_TakeOrderState : BaseState
{
    private EmployeeController employee;

    public Employee_TakeOrderState(EmployeeController employee)
    {
        this.employee = employee;
    }

    public override void OnStateEnter()
    {
        // 걷기 애니메이션 재생
    }

    public override void OnStateUpdate()
    {
        // 카운터에 음식 있으면
    }

    public override void OnStateExit()
    {

    }
}
