using System;
using UnityEngine;

public abstract class StatusEffectCheckState : State
{
    public StatusEffectCheckState(StateMachine _stateMachine) : base(_stateMachine)
    {

    }

    protected bool CheckForStatusEffects(BattleStatusEffectsManager statusManager, BattleLogic battleLogic, BattleTextBoxHandler textBoxHandler, StatsManager inhibitor, Enum currentState)
    {
        Debug.Log("StatusCheck");
        if (statusManager.CheckForStatusEffect(battleLogic, inhibitor))
        {
            if (!inhibitor.HealthManager.Dead)
            {
                StatusEffectAnimationState animState = (StatusEffectAnimationState)stateMachine.states[BattleStates.StatusEffectAnimations];
                animState.stateToReturnToo = currentState;
            }
            return true;
        }
        else
        {
            Debug.Log("There were no status effects");
            return false;
        }
    }
}
