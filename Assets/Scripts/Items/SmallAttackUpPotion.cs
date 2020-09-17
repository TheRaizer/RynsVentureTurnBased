using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAttackUpPotion : Useables
{
    [SerializeField] private AttackUp attackUpEffect = null;

    public override void OnUse(StatsManager statsManager, StateMachine battleStateMachine, TextBoxHandler textBoxHandler)
    {
        base.OnUse(statsManager, battleStateMachine, textBoxHandler);

        statsManager.StatusEffectsManager.AddToStatusEffectsDic(EffectType.SingleTurnTrigger, attackUpEffect, textBoxHandler);
        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }
}
