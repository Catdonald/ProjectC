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
        // �ȱ� �ִϸ��̼� ���
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnStateExit()
    {

    }
}
