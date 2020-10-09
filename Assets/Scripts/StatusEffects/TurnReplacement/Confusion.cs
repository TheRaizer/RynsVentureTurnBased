using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : StatusEffect
{
    public override void OnTurn(BattleHandler battleLogic, StatsManager currentUser, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        base.OnTurn(battleLogic, currentUser, battleStateMachine, textBoxHandler);

        Debug.Log("confusion");
        battleLogic.CheckForAttackablePlayers();
        battleLogic.CheckForEnemiesRemaining();
        EntityType teamTypeToAttack = Random.Range(0, 2) == 0 ? EntityType.Player : EntityType.Enemy;

        AnimatedVer = currentUser.user.Animator.gameObject;//the object we will be animating is the current user whose turn will be replaced

        if (currentUser.user.EntityType == EntityType.Player)
        {
            List<StatsManager> possibleEntitiesToAttack = battleLogic.AttackablesDic[teamTypeToAttack];
            AnimToPlay = battleLogic.CurrentPlayerAttack.AnimToPlay;
            TriggerName = battleLogic.CurrentPlayerAttack.TriggerName;

            if (battleLogic.CurrentPlayerAttack.IsAOE)
            {
                battleLogic.CurrentPlayerAttack.UseAOEAction(possibleEntitiesToAttack, battleLogic.CurrentPlayer.Stats.DamageScale, textBoxHandler);
            }
            else
            {
                int entityToHit = Random.Range(0, battleLogic.AttackablesDic[teamTypeToAttack].Count);
                battleLogic.CurrentPlayerAttack.UseAction(possibleEntitiesToAttack[entityToHit], battleLogic.CurrentPlayer.Stats.DamageScale, textBoxHandler);
            }
        }
        else
        {
            EntityAction attackToUse = battleLogic.CurrentEnemy.Attacks[Random.Range(0, battleLogic.CurrentEnemy.Attacks.Count)];
            AnimToPlay = attackToUse.AnimToPlay;
            TriggerName = attackToUse.TriggerName;

            if (attackToUse.IsAOE)
            {
                attackToUse.UseAOEAction(battleLogic.AttackablesDic[teamTypeToAttack], currentUser.DamageScale, textBoxHandler);
            }
            else
            {
                int indexToAttack = Random.Range(0, battleLogic.AttackablesDic[teamTypeToAttack].Count);
                attackToUse.UseAction(battleLogic.AttackablesDic[teamTypeToAttack][indexToAttack], currentUser.DamageScale, textBoxHandler);
            }
        }
    }

    public override StatusEffect ShallowCopy()
    {
        return (Confusion)MemberwiseClone();
    }
}
