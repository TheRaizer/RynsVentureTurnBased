using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : StatusEffect
{
    public override void OnTurn(BattleLogic battleLogic, StatsManager currentUser, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        base.OnTurn(battleLogic, currentUser, battleStateMachine, textBoxHandler);

        Debug.Log("confusion");
        battleLogic.CheckForAttackablePlayers();
        battleLogic.CheckForEnemiesRemaining();
        EntityType teamType = Random.Range(0, 2) == 0 ? EntityType.Player : EntityType.Enemy;

        AnimatedVer = currentUser.user.Animator.gameObject;//the object we will be animating is the current user whose turn will be replaced

        if (currentUser.user.EntityType == EntityType.Player)
        {
            int entityToHit = Random.Range(0, battleLogic.AttackablesDic[teamType].Count);

            StatsManager entityToAttack = battleLogic.AttackablesDic[teamType][entityToHit];
            AnimToPlay = battleLogic.CurrentPlayerAttack.AnimToPlay;
            TriggerName = battleLogic.CurrentPlayerAttack.TriggerName;

            EntityActionInfo attackInfo = battleLogic.CurrentPlayerAttack.UseAction(entityToAttack, battleLogic.CurrentPlayer.Stats.DamageScale, textBoxHandler);
        }
        else
        {
            int playerIndexToAttack = Random.Range(0, battleLogic.AttackablesDic[teamType].Count);
            EntityAction attackToUse = battleLogic.CurrentEnemy.Attacks[Random.Range(0, battleLogic.CurrentEnemy.Attacks.Count)];
            AnimToPlay = attackToUse.AnimToPlay;
            TriggerName = attackToUse.TriggerName;

            List<EntityActionInfo> attackInfos = attackToUse.DetermineAction(battleLogic.AttackablesDic[teamType], battleLogic.CurrentEnemy.Stats.DamageScale, textBoxHandler, playerIndexToAttack);
        }
    }

    public override StatusEffect ShallowCopy()
    {
        return (Confusion)MemberwiseClone();
    }
}
