using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCommand : BattleCommands
{
    private void Awake()
    {
        actionOnPress = Fight;
    }

    private void Fight(BattleLogic battleLogic)
    {
        battleLogic.CurrentPlayerAttack = battleLogic.CurrentPlayer.FightAttack;
        battleLogic.battleStateMachine.ChangeState(BattleStates.EnemyChoice);
    }
}
