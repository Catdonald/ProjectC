using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Customer_LineMoveState : BaseState
{
    public Customer_LineMoveState(CustomerController customer)
    {
        this.customer = customer;
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
    private CustomerController customer;
}
