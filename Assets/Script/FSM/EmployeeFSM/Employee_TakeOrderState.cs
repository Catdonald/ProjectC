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
        // �ȱ� �ִϸ��̼� ���
    }

    public override void OnStateUpdate()
    {
        // ī���Ϳ� ���� ������
    }

    public override void OnStateExit()
    {

    }
}
