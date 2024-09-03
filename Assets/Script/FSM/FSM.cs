using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private BaseState currentState;
    public FSM(BaseState initState)
    {
        currentState = initState;
        ChangeState(currentState);
    }
    public void ChangeState(BaseState nextState)
    {
        if (currentState == nextState)
            return;

        if(currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = nextState;
        currentState.OnStateEnter();
    }
    public void UpdateState()
    {
        if(currentState != null)
        {
            currentState.OnStateUpdate();
        }
    }
}
