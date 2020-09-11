using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : StatusEffect
{
    public override void OnTurn(BattleLogic battleLogic, StatsManager currentUser, StateMachine battleStateMachine, TextBoxHandler textBoxHandler)
    {
        base.OnTurn(battleLogic, currentUser, battleStateMachine, textBoxHandler);
        Debug.Log("confusion");
        battleLogic.CheckForAttackablePlayers();
        EntityType teamType = Random.Range(0, 2) == 0 ? EntityType.Player : EntityType.Enemy;

        if (currentUser.user.EntityType == EntityType.Player)
        {
            int entityToHit = Random.Range(0, battleLogic.AttackablesDic[teamType].Count);

            StatsManager entityToAttack = battleLogic.AttackablesDic[teamType][entityToHit];
            EntityActionInfo attackInfo = battleLogic.CurrentPlayerAttack.UseAttack(entityToAttack, battleLogic.CurrentPlayer.Stats.DamageScale);

            textBoxHandler.AddTextAsAttack(battleLogic.CurrentPlayer.Id, battleLogic.CurrentPlayerAttack.AttackText, entityToAttack.user.Id);

            if (!attackInfo.hitTarget)
            {
                textBoxHandler.AddTextOnMiss(battleLogic.CurrentPlayer.Id, attackInfo.targetId);
            }
            else if (battleLogic.CurrentPlayerAttack.WasCriticalHit)
            {
                textBoxHandler.AddTextAsCriticalHit();
            }
        }
        else
        {
            int playerIndexToAttack = Random.Range(0, battleLogic.AttackablesDic[teamType].Count);
            EntityAction attackToUse = battleLogic.CurrentEnemy.Attacks[Random.Range(0, battleLogic.CurrentEnemy.Attacks.Count)];

            List<EntityActionInfo> attackInfos = attackToUse.DetermineAttack(battleLogic.AttackablesDic[teamType], battleLogic.CurrentEnemy.Stats.DamageScale, playerIndexToAttack);
            textBoxHandler.GenerateEnemyText(attackInfos, attackToUse);
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
