using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : StatusEffect
{
    public override void OnTurn(BattleHandler battleHandler, StatsManager currentUser, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        base.OnTurn(battleHandler, currentUser, battleStateMachine, textBoxHandler);

        Debug.Log("confusion");
        BattleEntitiesManager battleEntitiesManager = battleHandler.BattleEntitiesManager;
        battleEntitiesManager.CheckForAttackablePlayers();
        battleEntitiesManager.CheckForEnemiesRemaining();
        EntityType teamTypeToAttack = Random.Range(0, 2) == 0 ? EntityType.Player : EntityType.Enemy;

        AnimatedVer = currentUser.user.Animator.gameObject;//the object we will be animating is the current user whose turn will be replaced

        if (currentUser.user.EntityType == EntityType.Player)
        {
            List<StatsManager> possibleEntitiesToAttack = battleEntitiesManager.AttackablesDic[teamTypeToAttack];
            AnimToPlay = battleEntitiesManager.CurrentPlayerAttack.AnimToPlay;
            TriggerName = battleEntitiesManager.CurrentPlayerAttack.TriggerName;

            if (battleEntitiesManager.CurrentPlayerAttack.IsAOE)
            {
                battleEntitiesManager.CurrentPlayerAttack.UseAOEAction(possibleEntitiesToAttack, currentUser.DamageScale, textBoxHandler);
            }
            else
            {
                int entityToHit = Random.Range(0, battleEntitiesManager.AttackablesDic[teamTypeToAttack].Count);
                battleEntitiesManager.CurrentPlayerAttack.UseAction(possibleEntitiesToAttack[entityToHit], currentUser.DamageScale, textBoxHandler);
            }
        }
        else
        {
            EntityAction attackToUse = battleEntitiesManager.CurrentEnemy.Attacks[Random.Range(0, battleEntitiesManager.CurrentEnemy.Attacks.Count)];
            AnimToPlay = attackToUse.AnimToPlay;
            TriggerName = attackToUse.TriggerName;

            if (attackToUse.IsAOE)
            {
                attackToUse.UseAOEAction(battleEntitiesManager.AttackablesDic[teamTypeToAttack], currentUser.DamageScale, textBoxHandler);
            }
            else
            {
                int indexToAttack = Random.Range(0, battleEntitiesManager.AttackablesDic[teamTypeToAttack].Count);
                attackToUse.UseAction(battleEntitiesManager.AttackablesDic[teamTypeToAttack][indexToAttack], currentUser.DamageScale, textBoxHandler);
            }
        }
    }

    public override StatusEffect ShallowCopy()
    {
        return (Confusion)MemberwiseClone();
    }
}
