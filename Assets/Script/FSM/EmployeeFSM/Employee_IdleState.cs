using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee_IdleState : BaseState
{
    private EmployeeController employee;

    public Employee_IdleState(EmployeeController employee)
    { 
        this.employee = employee; 
    }

    public override void OnStateEnter()
    {
        // 아이들 애니메이션 재생
    }

    public override void OnStateUpdate()
    {
        // 할 일 찾기
    }

    public override void OnStateExit()
    {
        
    }
}
