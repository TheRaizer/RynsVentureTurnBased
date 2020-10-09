using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    public override void OnTurn(BattleHandler battleLogic, StatsManager currentUser, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        base.OnTurn(battleLogic, currentUser, battleStateMachine, textBoxHandler);
        Debug.Log("Skip Turn");
        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    public override StatusEffect ShallowCopy()
    {
        return (Stun)MemberwiseClone();
    }
}
