using System.Collections.Generic;
using UnityEngine;

public class EnemyChoiceState : State
{
    private readonly BattleHandler battleHandler;
    public readonly VectorMenuTraversal menuTraversal;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleStatusEffectsManager battleStatusManager;
    private readonly BattleEntitiesManager battleEntitiesManager;
    private readonly BattleAnimationsHandler animationsHandler;

    public EnemyChoiceState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleTextBoxHandler _textBoxHandler, BattleStatusEffectsManager _battleStatusManager) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        textBoxHandler = _textBoxHandler;
        battleStatusManager = _battleStatusManager;
        battleEntitiesManager = battleHandler.BattleEntitiesManager;
        animationsHandler = battleHandler.AnimationsHandler;

        menuTraversal = new VectorMenuTraversal(PositionPointerForEnemyChoice)
        {
            MaxIndex = battleEntitiesManager.Enemies.Length - 1
        };
    }

    
    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        SetPointerToValidPosition();
        animationsHandler.OnAnimationFinished = OnAnimationFinished;
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        if (battleStatusManager.CheckForReplacementStatusEffect(battleHandler, battleEntitiesManager.CurrentPlayer.Stats, true))
        {
            stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!animationsHandler.RanAnim)
        {
            menuTraversal.TraverseWithNulls(battleEntitiesManager.Enemies);
            CheckIfEnterSelected();
            CheckIfExitSelected();
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        animationsHandler.CheckIfAnimationFinished();
    }

    private void SetPointerToValidPosition()
    {
        while (battleEntitiesManager.Enemies[menuTraversal.currentIndex] == null)
        {
            menuTraversal.currentIndex++;
            menuTraversal.CheckIfIndexInRange();
        }
        PositionPointerForEnemyChoice();
    }

    private void CheckIfExitSelected()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(BattleStates.FightMenu);
        }
    }

    private void CheckIfEnterSelected()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E) && !animationsHandler.RanAnim)
        {
            Enemy enemyToAttack = battleEntitiesManager.Enemies[menuTraversal.currentIndex].GetComponent<Enemy>();
            float damageScale = battleEntitiesManager.CurrentPlayer.Stats.DamageScale;
            EntityActionInfo attackInfo = battleEntitiesManager.CurrentPlayerAttack.UseAction(enemyToAttack.Stats, damageScale, textBoxHandler);
                
            battleHandler.SetActionPopupForEntity(battleEntitiesManager.CurrentPlayer.Animator.gameObject, null, enemyToAttack.Animator.gameObject, attackInfo);
            Animator animator = battleEntitiesManager.CurrentPlayer.Stats.user.Animator;
            AnimationClip animToPlay = battleEntitiesManager.CurrentPlayerAttack.AnimToPlay;
            string triggerName = battleEntitiesManager.CurrentPlayerAttack.TriggerName;

            animationsHandler.RunAnim(animator, animToPlay, triggerName);
            return;
        }
    }

    private void OnAnimationFinished()
    {
        battleEntitiesManager.CheckForEnemiesRemaining();
        battleHandler.TextMods.ChangeEnemyNameColour();
        battleHandler.BattleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    private void PositionPointerForEnemyChoice()
    {
        BattleMenusHandler menusHandler = battleHandler.MenusHandler;
        menusHandler.PositionPointer
            (
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].top,
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].bottom,
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].left,
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].right
            );
    }
}
