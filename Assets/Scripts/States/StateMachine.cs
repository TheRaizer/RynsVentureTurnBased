using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State CurrentState { get; private set; }

    public Dictionary<Enum, State> states = new Dictionary<Enum, State>();

    public void Initialize(Dictionary<Enum, State> _states, Enum startState)
    {
        states = _states;

        if (states.ContainsKey(startState))
            CurrentState = states[startState];
        else
            Debug.Log("State of type " + startState + " is not available");
    }

    public void ReturnBackToState(Enum stateToReturnToo)
    {
        if (!states.ContainsKey(stateToReturnToo))
        {
            Debug.Log("StateMachine does not contain " + stateToReturnToo);
            return;
        }
        CurrentState.OnExit();
        CurrentState = states[stateToReturnToo];
        CurrentState.OnEnterOrReturn();
    }

    public void ChangeState(Enum stateToChangeToo)
    {
        if (!states.ContainsKey(stateToChangeToo))
        {
            Debug.Log("StateMachine does not contain " + stateToChangeToo);
            return;
        }

        CurrentState.OnExit();
        CurrentState = states[stateToChangeToo];
        CurrentState.OnEnterOrReturn();
        CurrentState.OnFullRotationEnter();
    }
}
