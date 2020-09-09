using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    public override void OnTurn(List<StatsManager> attackableTeam, List<StatsManager> opposingTeam, StatsManager currentUser, BattleLogic battleLogic)
    {
        base.OnTurn(attackableTeam, opposingTeam, currentUser, battleLogic);
        Debug.Log("Calculate Next Turn");
        battleLogic.CalculateNextTurn();
    }
}
