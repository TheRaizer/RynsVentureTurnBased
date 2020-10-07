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

        battleLogic.AnimationsHandler.OnAnimationFinished = OnAnimationFinished;
        Debug.Log(battleLogic.CurrentEnemy.Id + " Turn");

        if (battleStatusManager.CheckForReplacementStatusEffect(battleLogic, battleLogic.CurrentEnemy.Stats, true))
        {
            StatusEffectAnimationState animState = (StatusEffectAnimationState)stateMachine.states[BattleStates.StatusEffectAnimations];
            animState.stateToReturnToo = BattleStates.FightMenu;
            stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
            return;
        }
        battleLogic.CheckForAttackablePlayers();

        ManageAttackToUse();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        battleLogic.AnimationsHandler.OnLateUpdate();
    }

    private void OnAnimationFinished()
    {
        if (CheckForStatusEffects(battleStatusManager, battleLogic, textBoxHandler, battleLogic.CurrentEnemy.Stats, null))
        {
            stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
            return;
        }
        stateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    private void ManageAttackToUse()
    {
        EntityAction attackToUse = battleLogic.CurrentEnemy.Attacks[Random.Range(0, battleLogic.CurrentEnemy.Attacks.Count)];
        if (attackToUse.IsAOE)
        {
            AOEState aoeState = (AOEState)stateMachine.states[BattleStates.AOEState];
            aoeState.ActionToUse = attackToUse;
            aoeState.EntityUsing = EntityType.Enemy;
            stateMachine.ChangeState(BattleStates.AOEState);
            return;
        }
        if (attackToUse.ActionType == EntityAction.ActionTypes.Support)
        {
            int enemyIndexToSupport = Random.Range(0, battleLogic.AttackablesDic[EntityType.Enemy].Count);
            List<EntityActionInfo> actionInfos = attackToUse.DetermineAction(battleLogic.AttackablesDic[EntityType.Enemy], battleLogic.CurrentEnemy.Stats.DamageScale, textBoxHandler, enemyIndexToSupport);
        }
        else
        {
            int playerIndexToAttack = Random.Range(0, battleLogic.AttackablesDic[EntityType.Player].Count);
            List<EntityActionInfo> actionInfos = attackToUse.DetermineAction(battleLogic.AttackablesDic[EntityType.Player], battleLogic.CurrentEnemy.Stats.DamageScale, textBoxHandler, playerIndexToAttack);
        }
        battleLogic.TextMods.PrintPlayerHealth();
        battleLogic.TextMods.ChangePlayerTextColour();
        Debug.Log("printing enemy attacks");

        battleLogic.AnimationsHandler.RunAnim(battleLogic.CurrentEnemy.Animator, attackToUse.AnimToPlay, attackToUse.TriggerName);
    }
}
