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
        // ���ı�� �տ��� 2�� ���� ���
        // ����� �� ������ 1�� �̻� ������ ������
        // ���� ������ �����δ�.
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}
