using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallHpPotion : Useables
{
    [SerializeField] private int amountToHeal = 5;

    public override void OnUse(StatsManager statsManager, StateMachine battleStateMachine, TextBoxHandler textBoxHandler)
    {
        base.OnUse(statsManager, battleStateMachine, textBoxHandler);

        statsManager.HealthManager.RegenAmount(amountToHeal);
        textBoxHandler.AddTextAsUseable(statsManager.user.Id, id);
        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }
}
