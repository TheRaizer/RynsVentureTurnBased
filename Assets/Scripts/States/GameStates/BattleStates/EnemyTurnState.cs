using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : State
{
    private readonly BattleHandler battleHandler;
    private readonly BattleStatusEffectsManager battleStatusManager;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleEntitiesManager battleEntitiesManager;
    private readonly BattleAnimationsHandler animationsHandler;
    private bool checkedForStatusEffects = false;

    public EnemyTurnState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleStatusEffectsManager _battleStatusManager, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        battleStatusManager = _battleStatusManager;
        textBoxHandler = _textBoxHandler;
        battleEntitiesManager = battleHandler.BattleEntitiesManager;
        animationsHandler = battleHandler.AnimationsHandler;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        animationsHandler.OnAnimationFinished = OnAnimationFinished;
        Debug.Log(battleEntitiesManager.CurrentEnemy.Id + " Turn");
        if (CheckForStatusEffects()) return;
        battleEntitiesManager.CheckForAttackablePlayers();
        if (battleEntitiesManager.CurrentEnemy.Stats.HealthManager.Dead)
        {
            battleEntitiesManager.CalculateNextTurn();
            return;
        }
        ManageAttackToUse();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        animationsHandler.CheckIfAnimationFinished();
    }

    private void OnAnimationFinished()
    {
        stateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    private bool CheckForStatusEffects()
    {
        StatusEffectAnimationState animState = (StatusEffectAnimationState)stateMachine.states[BattleStates.StatusEffectAnimations];
        if (!checkedForStatusEffects)
        {
            if (battleStatusManager.CheckForStatusEffect(battleHandler, battleEntitiesManager.CurrentEnemy.Stats))
            {
                checkedForStatusEffects = true;
                animState.StateToReturnToo = BattleStates.EnemyTurn;
                stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
                return true;
            }
        }
        else
        {
            checkedForStatusEffects = false;
        }

        if (battleStatusManager.CheckForReplacementStatusEffect(battleHandler, battleEntitiesManager.CurrentEnemy.Stats, true))
        {
            animState.RunReplacementAnimation();
            return true;
        }
        return false;
    }

    private void ManageAttackToUse()
    {
        EntityAction attackToUse = battleEntitiesManager.CurrentEnemy.Attacks[Random.Range(0, battleEntitiesManager.CurrentEnemy.Attacks.Count)];
        if (attackToUse.IsAOE)
        {
            AOEState aoeState = (AOEState)stateMachine.states[BattleStates.AOEState];
            aoeState.ActionToUse = attackToUse;
            aoeState.EntityUsing = EntityType.Enemy;
            stateMachine.ChangeState(BattleStates.AOEState);
            return;
        }
        else
        {
            if (attackToUse.ActionType == EntityAction.ActionTypes.Support || attackToUse.ActionType == EntityAction.ActionTypes.Revive)
            {
                int enemyIndexToSupport = Random.Range(0, battleEntitiesManager.AttackablesDic[EntityType.Enemy].Count);
                StatsManager enemyToSupport = battleEntitiesManager.AttackablesDic[EntityType.Enemy][enemyIndexToSupport];

                attackToUse.UseAction(enemyToSupport, battleEntitiesManager.CurrentEnemy.Stats.DamageScale, textBoxHandler);
            }
            else
            {
                int playerIndexToAttack = Random.Range(0, battleEntitiesManager.AttackablesDic[EntityType.Player].Count);
                StatsManager playerToAttack = battleEntitiesManager.AttackablesDic[EntityType.Player][playerIndexToAttack];

                attackToUse.UseAction(playerToAttack, battleEntitiesManager.CurrentEnemy.Stats.DamageScale, textBoxHandler);
            }
        }

        CheckForTextMods();
        Debug.Log("printing enemy attacks");
        Debug.Log(attackToUse.AnimToPlay);
        animationsHandler.RunAnim(battleEntitiesManager.CurrentEnemy.Animator, attackToUse.AnimToPlay, attackToUse.TriggerName);
    }

    private void CheckForTextMods()
    {
        battleHandler.TextMods.PrintPlayerHealth();
        battleHandler.TextMods.ChangePlayerTextColour();
    }
}
