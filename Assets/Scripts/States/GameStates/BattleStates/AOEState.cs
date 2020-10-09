using System.Collections.Generic;
using UnityEngine;

public class AOEState : State
{
    private readonly BattleHandler battleHandler;
    private readonly BattleTextBoxHandler textBoxHandler;
    public EntityType EntityUsing { get; set; }
    public EntityAction ActionToUse { get; set; }

    public AOEState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        textBoxHandler = _textBoxHandler;
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        DecideActionToUse();
    }

    

    public override void InputUpdate()
    {
        base.InputUpdate();

        battleHandler.AnimationsHandler.CheckIfAnimationFinished();
    }

    private void UseActionOnPlayers()
    {
        ActionToUse.UseAOEAction(battleHandler.AttackablesDic[EntityType.Player], battleHandler.CurrentEnemy.Stats.DamageScale, textBoxHandler);
    }

    private void UseActionOnEnemies()//popups currently only work on enemies
    {
        List<EntityActionInfo> actionInfos = ActionToUse.UseAOEAction(battleHandler.AttackablesDic[EntityType.Enemy], battleHandler.CurrentPlayer.Stats.DamageScale, textBoxHandler);
        List<GameObject> enemyBattleVersions = new List<GameObject>();
        foreach(StatsManager s in battleHandler.AttackablesDic[EntityType.Enemy])
        {
            enemyBattleVersions.Add(s.user.Animator.gameObject);
        }

        battleHandler.SetActionPopupForEntity(battleHandler.CurrentPlayer.Animator.gameObject, enemyBattleVersions, null, null, actionInfos);
    }
    private void OnAnimationFinishedPlayer()
    {
        battleHandler.CheckForEnemiesRemaining();
        battleHandler.TextMods.ChangeEnemyNameColour();
        battleHandler.BattleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }
    private void DecideActionToUse()
    {
        if (EntityUsing == EntityType.Player)
        {
            battleHandler.AnimationsHandler.OnAnimationFinished = OnAnimationFinishedPlayer;
            if (ActionToUse.ActionType == EntityAction.ActionTypes.Support || ActionToUse.ActionType == EntityAction.ActionTypes.Revive)
            {
                UseActionOnPlayers();
            }
            else if (ActionToUse.ActionType == EntityAction.ActionTypes.Attack)
            {
                UseActionOnEnemies();
            }
            battleHandler.AnimationsHandler.RunAnim(battleHandler.CurrentPlayer.Animator, ActionToUse.AnimToPlay, ActionToUse.TriggerName);
        }
        else if (EntityUsing == EntityType.Enemy)
        {
            if (ActionToUse.ActionType == EntityAction.ActionTypes.Support || ActionToUse.ActionType == EntityAction.ActionTypes.Revive)
            {
                UseActionOnEnemies();
            }
            else if (ActionToUse.ActionType == EntityAction.ActionTypes.Attack)
            {
                UseActionOnPlayers();
            }
            battleHandler.AnimationsHandler.RunAnim(battleHandler.CurrentEnemy.Animator, ActionToUse.AnimToPlay, ActionToUse.TriggerName);
        }
    }
}
