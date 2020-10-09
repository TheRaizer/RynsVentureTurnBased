using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : State
{
    private readonly BattleHandler battleHandler;
    private readonly BattleStatusEffectsManager battleStatusManager;
    private readonly BattleTextBoxHandler textBoxHandler;
    private bool checkedForStatusEffects = false;

    public EnemyTurnState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleStatusEffectsManager _battleStatusManager, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        battleStatusManager = _battleStatusManager;
        textBoxHandler = _textBoxHandler;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        battleHandler.AnimationsHandler.OnAnimationFinished = OnAnimationFinished;
        Debug.Log(battleHandler.CurrentEnemy.Id + " Turn");
        if (CheckForStatusEffects()) return;
        battleHandler.CheckForAttackablePlayers();

        ManageAttackToUse();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        battleHandler.AnimationsHandler.CheckIfAnimationFinished();
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
            if (battleStatusManager.CheckForStatusEffect(battleHandler, battleHandler.CurrentEnemy.Stats))
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

        if (battleStatusManager.CheckForReplacementStatusEffect(battleHandler, battleHandler.CurrentEnemy.Stats, true))
        {
            animState.RunReplacementAnimation();
            return true;
        }
        return false;
    }

    private void ManageAttackToUse()
    {
        EntityAction attackToUse = battleHandler.CurrentEnemy.Attacks[Random.Range(0, battleHandler.CurrentEnemy.Attacks.Count)];
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
                int enemyIndexToSupport = Random.Range(0, battleHandler.AttackablesDic[EntityType.Enemy].Count);
                StatsManager enemyToSupport = battleHandler.AttackablesDic[EntityType.Enemy][enemyIndexToSupport];

                attackToUse.UseAction(enemyToSupport, battleHandler.CurrentEnemy.Stats.DamageScale, textBoxHandler);
            }
            else
            {
                int playerIndexToAttack = Random.Range(0, battleHandler.AttackablesDic[EntityType.Player].Count);
                StatsManager playerToAttack = battleHandler.AttackablesDic[EntityType.Player][playerIndexToAttack];

                attackToUse.UseAction(playerToAttack, battleHandler.CurrentEnemy.Stats.DamageScale, textBoxHandler);
            }
        }

        CheckForTextMods();
        Debug.Log("printing enemy attacks");
        Debug.Log(attackToUse.AnimToPlay);
        battleHandler.AnimationsHandler.RunAnim(battleHandler.CurrentEnemy.Animator, attackToUse.AnimToPlay, attackToUse.TriggerName);
    }

    private void CheckForTextMods()
    {
        battleHandler.TextMods.PrintPlayerHealth();
        battleHandler.TextMods.ChangePlayerTextColour();
    }
}
