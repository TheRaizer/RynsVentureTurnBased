using System;
using UnityEngine;

public abstract class StatusEffectCheckState : State
{
    public abstract bool CheckedStatusEffectThisTurn { get; set; }
    public StatusEffectCheckState(StateMachine _stateMachine) : base(_stateMachine)
    {

    }

    protected bool CheckForStatusEffects(BattleStatusEffectsManager statusManager, BattleLogic battleLogic, TextBoxHandler textBoxHandler, StatsManager inhibitor, Enum currentState)
    {
        if (statusManager.CheckForStatusEffect(battleLogic.AttackablesDic, inhibitor))
        {
            if (!inhibitor.HealthManager.Dead)
            {
                textBoxHandler.PreviousState = currentState;
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
