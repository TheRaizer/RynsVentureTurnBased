using System.Collections.Generic;
using UnityEngine;

public class AOEState : State
{
    private readonly BattleHandler battleHandler;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleEntitiesManager battleEntitiesManager;
    private readonly BattleAnimationsHandler animationsHandler;
    public EntityType EntityUsing { get; set; }
    public EntityAction ActionToUse { get; set; }

    public AOEState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        textBoxHandler = _textBoxHandler;
        battleEntitiesManager = battleHandler.BattleEntitiesManager;
        animationsHandler = battleHandler.AnimationsHandler;
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        DecideActionToUse();
    }

    

    public override void InputUpdate()
    {
        base.InputUpdate();

        animationsHandler.CheckIfAnimationFinished();
    }

    private void UseActionOnPlayers()
    {
        float damageScale = battleEntitiesManager.CurrentEnemy.Stats.DamageScale;
        ActionToUse.UseAOEAction(battleEntitiesManager.AttackablesDic[EntityType.Player], damageScale, textBoxHandler);
    }

    private void UseActionOnEnemies()//popups currently only work on enemies
    {
        float damageScale = battleEntitiesManager.CurrentPlayer.Stats.DamageScale;
        List<EntityActionInfo> actionInfos = ActionToUse.UseAOEAction(battleEntitiesManager.AttackablesDic[EntityType.Enemy], damageScale, textBoxHandler);
        List<GameObject> enemyBattleVersions = new List<GameObject>();
        foreach(StatsManager s in battleEntitiesManager.AttackablesDic[EntityType.Enemy])
        {
            enemyBattleVersions.Add(s.user.Animator.gameObject);
        }

        battleHandler.SetActionPopupForEntity(battleEntitiesManager.CurrentPlayer.Animator.gameObject, enemyBattleVersions, null, null, actionInfos);
    }
    private void OnAnimationFinishedPlayer()
    {
        battleEntitiesManager.CheckForEnemiesRemaining();
        battleHandler.TextMods.ChangeEnemyNameColour();
        battleHandler.BattleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }
    private void DecideActionToUse()
    {
        if (EntityUsing == EntityType.Player)
        {
            animationsHandler.OnAnimationFinished = OnAnimationFinishedPlayer;
            if (ActionToUse.ActionType == EntityAction.ActionTypes.Support || ActionToUse.ActionType == EntityAction.ActionTypes.Revive)
            {
                UseActionOnPlayers();
            }
            else if (ActionToUse.ActionType == EntityAction.ActionTypes.Attack)
            {
                UseActionOnEnemies();
            }
            animationsHandler.RunAnim(battleEntitiesManager.CurrentPlayer.Animator, ActionToUse.AnimToPlay, ActionToUse.TriggerName);
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
            animationsHandler.RunAnim(battleEntitiesManager.CurrentEnemy.Animator, ActionToUse.AnimToPlay, ActionToUse.TriggerName);
        }
    }
}
