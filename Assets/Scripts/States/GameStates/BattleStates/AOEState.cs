using System.Collections.Generic;
using UnityEngine;

public class AOEState : State
{
    private readonly BattleLogic battleLogic;
    private readonly BattleTextBoxHandler textBoxHandler;
    public EntityType EntityUsing { get; set; }
    public EntityAction ActionToUse { get; set; }

    public AOEState(StateMachine _stateMachine, BattleLogic _battleLogic, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        textBoxHandler = _textBoxHandler;
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        if(EntityUsing == EntityType.Player)
        {
            battleLogic.AnimationsHandler.OnAnimationFinished = OnAnimationFinishedPlayer;
            if (ActionToUse.ActionType == EntityAction.ActionTypes.Support || ActionToUse.ActionType == EntityAction.ActionTypes.Revive)
            {
                UseActionOnPlayers();
            }
            else if(ActionToUse.ActionType == EntityAction.ActionTypes.Attack)
            {
                UseActionOnEnemies();
            }
            battleLogic.AnimationsHandler.RunAnim(battleLogic.CurrentPlayer.Animator, ActionToUse.AnimToPlay, ActionToUse.TriggerName);
        }
        else if(EntityUsing == EntityType.Enemy)
        {
            if (ActionToUse.ActionType == EntityAction.ActionTypes.Support || ActionToUse.ActionType == EntityAction.ActionTypes.Revive)
            {
                UseActionOnEnemies();
            }
            else if(ActionToUse.ActionType == EntityAction.ActionTypes.Attack)
            {
                UseActionOnPlayers();
            }
            battleLogic.AnimationsHandler.RunAnim(battleLogic.CurrentEnemy.Animator, ActionToUse.AnimToPlay, ActionToUse.TriggerName);
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        battleLogic.AnimationsHandler.OnLateUpdate();
    }

    private void UseActionOnPlayers()
    {
        List<EntityActionInfo> actionInfos = ActionToUse.DetermineAction(battleLogic.AttackablesDic[EntityType.Player], battleLogic.CurrentEnemy.Stats.DamageScale, textBoxHandler);
    }

    private void UseActionOnEnemies()//popups currently only work on enemies
    {
        List<EntityActionInfo> actionInfos = ActionToUse.DetermineAction(battleLogic.AttackablesDic[EntityType.Enemy], battleLogic.CurrentPlayer.Stats.DamageScale, textBoxHandler);
        List<GameObject> enemyBattleVersions = new List<GameObject>();
        foreach(StatsManager s in battleLogic.AttackablesDic[EntityType.Enemy])
        {
            enemyBattleVersions.Add(s.user.Animator.gameObject);
        }

        battleLogic.SetActionPopupForEntity(battleLogic.CurrentPlayer.Animator.gameObject, enemyBattleVersions, null, null, actionInfos);
    }
    private void OnAnimationFinishedPlayer()
    {
        battleLogic.CheckForEnemiesRemaining();
        battleLogic.TextMods.ChangeEnemyNameColour();
        battleLogic.BattleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }
}
