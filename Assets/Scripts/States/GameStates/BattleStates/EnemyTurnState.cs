using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : StatusEffectCheckState
{
    private readonly BattleLogic battleLogic;
    private readonly BattleStatusEffectsManager battleStatusManager;
    private readonly BattleTextBoxHandler textBoxHandler;

    public EnemyTurnState(StateMachine _stateMachine, BattleLogic _battleLogic, BattleStatusEffectsManager _battleStatusManager, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        battleStatusManager = _battleStatusManager;
        textBoxHandler = _textBoxHandler;
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        Debug.Log(battleLogic.CurrentEnemy.Id + " Turn");
        textBoxHandler.AddTextAsTurn(battleLogic.CurrentEnemy.Id);
        stateMachine.ChangeState(BattleStates.BattleTextBox);

        if (CheckForStatusEffects(battleStatusManager, battleLogic, textBoxHandler, battleLogic.CurrentEnemy.Stats, null))
        {
            Debug.Log("printing status effects");
            stateMachine.ChangeState(BattleStates.BattleTextBox);
        }
        if (battleStatusManager.CheckForReplacementStatusEffect(battleLogic, battleLogic.CurrentEnemy.Stats, true))
        {
            return;
        }
        battleLogic.CheckForAttackablePlayers();

        EntityAction attackToUse = battleLogic.CurrentEnemy.Attacks[Random.Range(0, battleLogic.CurrentEnemy.Attacks.Count)];
        if (attackToUse.ActionType == EntityAction.ActionTypes.Support)
        {
            int enemyIndexToSupport = Random.Range(0, battleLogic.AttackablesDic[EntityType.Enemy].Count);
            List<EntityActionInfo> actionInfos = attackToUse.DetermineAction(battleLogic.AttackablesDic[EntityType.Enemy], battleLogic.CurrentEnemy.Stats.DamageScale, enemyIndexToSupport, textBoxHandler);
        }
        else
        {
            int playerIndexToAttack = Random.Range(0, battleLogic.AttackablesDic[EntityType.Player].Count);
            List<EntityActionInfo> actionInfos = attackToUse.DetermineAction(battleLogic.AttackablesDic[EntityType.Player], battleLogic.CurrentEnemy.Stats.DamageScale, playerIndexToAttack, textBoxHandler);
        }
        battleLogic.textMods.PrintPlayerHealth();
        battleLogic.textMods.ChangePlayerTextColour();
        Debug.Log("printing enemy attacks");
        stateMachine.ChangeState(BattleStates.BattleTextBox);
    }
}
