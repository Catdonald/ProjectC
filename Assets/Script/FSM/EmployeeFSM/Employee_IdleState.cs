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
        // ���̵� �ִϸ��̼� ���
    }

    public override void OnStateUpdate()
    {
        // �� �� ã��
    }

    public override void OnStateExit()
    {
        
    }
}
