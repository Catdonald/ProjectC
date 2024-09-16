using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee_MoveState : BaseState
{
    private EmployeeController employee;

    public Employee_MoveState(EmployeeController employee)
    {
        this.employee = employee;
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

    }
}
