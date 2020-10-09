using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCommand : BattleCommands
{
    private void Awake()
    {
        actionOnPress = ChangeToMagicState;
    }

    private void ChangeToMagicState(BattleHandler battleHandler)
    {
        if (battleHandler.CurrentPlayer.Magic.Count > 0)
        {
            battleHandler.BattleStateMachine.ChangeState(BattleStates.MagicChoice);
        }
    }
}
