using System;
using UnityEngine;

public abstract class StatusEffectCheckState : State
{
    public StatusEffectCheckState(StateMachine _stateMachine) : base(_stateMachine)
    {

    }

    protected bool CheckForStatusEffects(BattleStatusEffectsManager statusManager, BattleLogic battleLogic, StatsManager inhibitor, Enum currentState)
    {
        Debug.Log("StatusCheck");
        if (statusManager.CheckForStatusEffect(battleLogic, inhibitor))
        {
            return true;
        }
        else
        {
            Debug.Log("There were no status effects");
            return false;
        }
    }
}
