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
        EntityType teamType = Random.Range(0, 2) == 0 ? EntityType.Player : EntityType.Enemy;

        if (currentUser.user.EntityType == EntityType.Player)
        {
            int entityToHit = Random.Range(0, battleLogic.AttackablesDic[teamType].Count);

            StatsManager entityToAttack = battleLogic.AttackablesDic[teamType][entityToHit];
            EntityActionInfo attackInfo = battleLogic.CurrentPlayerAttack.UseAction(entityToAttack, battleLogic.CurrentPlayer.Stats.DamageScale, textBoxHandler);
        }
        else
        {
            int playerIndexToAttack = Random.Range(0, battleLogic.AttackablesDic[teamType].Count);
            EntityAction attackToUse = battleLogic.CurrentEnemy.Attacks[Random.Range(0, battleLogic.CurrentEnemy.Attacks.Count)];

            List<EntityActionInfo> attackInfos = attackToUse.DetermineAction(battleLogic.AttackablesDic[teamType], battleLogic.CurrentEnemy.Stats.DamageScale, playerIndexToAttack, textBoxHandler);
        }
        battleLogic.CheckForAttackablePlayers();
        battleLogic.CheckForEnemiesRemaining();

        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    public override StatusEffect ShallowCopy()
    {
        return (Confusion)MemberwiseClone();
    }
}
