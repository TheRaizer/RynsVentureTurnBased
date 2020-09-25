using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallHpPotion : Useable
{
    [SerializeField] private int amountToHeal = 7;

    public override void OnUseInBattle(StatsManager user, List<StatsManager> friendlyStats, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        base.OnUseInBattle(user, friendlyStats, battleStateMachine, textBoxHandler);

        user.HealthManager.RegenAmount(amountToHeal);
        textBoxHandler.AddTextAsUseable(user.user.Id, Id);
        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    public override bool OnlyHeal()
    {
        return true;
    }

    public override Useable ShallowClone()
    {
        return (SmallHpPotion)MemberwiseClone();
    }
}
