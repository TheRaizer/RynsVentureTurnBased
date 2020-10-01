using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAttackUp : Useable
{
    [SerializeField] private AttackUp attackUpEffect = null;

    public override void OnUseInBattle(StatsManager StatsToHeal, List<StatsManager> friendlyStats, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        base.OnUseInBattle(StatsToHeal, friendlyStats, battleStateMachine, textBoxHandler);

        StatsToHeal.StatusEffectsManager.AddToStatusEffectsDic(EffectType.SingleTurnTrigger, attackUpEffect);
        textBoxHandler.AddTextAsStatusInfliction("S Attack Up", StatsToHeal.user.Id, attackUpEffect.Name);
        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    public override Useable ShallowClone()
    {
        return (SmallAttackUp)MemberwiseClone();
    }
}
