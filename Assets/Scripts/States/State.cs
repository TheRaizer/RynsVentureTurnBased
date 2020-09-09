using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected StateMachine stateMachine;

    public State(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    public virtual void OnFullRotationEnter() { }
    public virtual void OnEnterOrReturn() { }
    public virtual void OnExit() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void InputUpdate() { }
}
