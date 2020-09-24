using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCommand : BattleCommands
{
    private void Awake()
    {
        actionOnPress = ChangeToMagicState;
    }

    private void ChangeToMagicState(BattleLogic battleLogic)
    {
        if (battleLogic.CurrentPlayer.Magic.Count > 0)
        {
            battleLogic.BattleStateMachine.ChangeState(BattleStates.MagicChoice);
        }
    }
}
