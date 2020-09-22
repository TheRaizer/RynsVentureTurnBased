using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAttackUp : Useable
{
    [SerializeField] private AttackUp attackUpEffect = null;

    public override void OnUseInBattle(StatsManager statsManager, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        base.OnUseInBattle(statsManager, battleStateMachine, textBoxHandler);

        statsManager.StatusEffectsManager.AddToStatusEffectsDic(EffectType.SingleTurnTrigger, attackUpEffect, textBoxHandler);
        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    public override Useable ShallowClone()
    {
        return (SmallAttackUp)MemberwiseClone();
    }
}
