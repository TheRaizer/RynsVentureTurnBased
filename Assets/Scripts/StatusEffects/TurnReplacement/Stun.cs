using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    public override void OnTurn(List<StatsManager> attackableTeam, List<StatsManager> opposingTeam, StatsManager currentUser, StateMachine battleStateMachine, TextBoxHandler textBoxHandler)
    {
        base.OnTurn(attackableTeam, opposingTeam, currentUser, battleStateMachine, textBoxHandler);
        Debug.Log("Skip Turn");
        textBoxHandler.PreviousState = null;
        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    public override StatusEffect ShallowCopy()
    {
        return (Stun)MemberwiseClone();
    }
}
