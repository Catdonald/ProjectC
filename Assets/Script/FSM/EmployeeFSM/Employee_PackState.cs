using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee_PackState : BaseState
{
    private EmployeeController employee;

    public Employee_PackState(EmployeeController employee)
    {
        this.employee = employee;
    }

    public override void OnStateEnter()
    {
        // ���� �ִϸ��̼� ���
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}
