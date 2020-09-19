using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallHpPotion : Useable
{
    [SerializeField] private int amountToHeal = 5;

    public override void OnUseInBattle(StatsManager statsManager, StateMachine battleStateMachine, TextBoxHandler textBoxHandler)
    {
        base.OnUseInBattle(statsManager, battleStateMachine, textBoxHandler);

        statsManager.HealthManager.RegenAmount(amountToHeal);
        textBoxHandler.AddTextAsUseable(statsManager.user.Id, Id);
        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    public override Useable ShallowClone()
    {
        return (SmallHpPotion)MemberwiseClone();
    }
}
