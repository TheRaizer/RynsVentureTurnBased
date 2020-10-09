using System.Collections.Generic;
using UnityEngine;

public class EnemyChoiceState : State
{
    private readonly BattleHandler battleHandler;
    private readonly BattleMenusHandler menusHandler;
    public readonly VectorMenuTraversal menuTraversal;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleStatusEffectsManager battleStatusManager;

    public EnemyChoiceState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleMenusHandler _menusHandler, BattleTextBoxHandler _textBoxHandler, BattleStatusEffectsManager _battleStatusManager) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        menusHandler = _menusHandler;
        textBoxHandler = _textBoxHandler;
        battleStatusManager = _battleStatusManager;

        menuTraversal = new VectorMenuTraversal(PositionPointerForEnemyChoice)
        {
            MaxIndex = battleHandler.Enemies.Length - 1
        };
    }

    
    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        SetPointerToValidPosition();
        battleHandler.AnimationsHandler.OnAnimationFinished = OnAnimationFinished;
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        if (battleStatusManager.CheckForReplacementStatusEffect(battleHandler, battleHandler.CurrentPlayer.Stats, true))
        {
            stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!battleHandler.AnimationsHandler.RanAnim)
        {
            menuTraversal.TraverseWithNulls(battleHandler.Enemies);
            CheckIfEnterSelected();
            CheckIfExitSelected();
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        battleHandler.AnimationsHandler.CheckIfAnimationFinished();
    }

    private void SetPointerToValidPosition()
    {
        while (battleHandler.Enemies[menuTraversal.currentIndex] == null)
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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E) && !battleHandler.AnimationsHandler.RanAnim)
        {
            Enemy enemyToAttack = battleHandler.Enemies[menuTraversal.currentIndex].GetComponent<Enemy>();
            EntityActionInfo attackInfo = battleHandler.CurrentPlayerAttack.UseAction(enemyToAttack.Stats, battleHandler.CurrentPlayer.Stats.DamageScale, textBoxHandler);
                
            battleHandler.SetActionPopupForEntity(battleHandler.CurrentPlayer.Animator.gameObject, null, enemyToAttack.Animator.gameObject, attackInfo);
            battleHandler.AnimationsHandler.RunAnim(battleHandler.CurrentPlayer.Stats.user.Animator, battleHandler.CurrentPlayerAttack.AnimToPlay, battleHandler.CurrentPlayerAttack.TriggerName);
            return;
        }
    }

    private void OnAnimationFinished()
    {
        battleHandler.CheckForEnemiesRemaining();
        battleHandler.TextMods.ChangeEnemyNameColour();
        battleHandler.BattleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    private void PositionPointerForEnemyChoice()
    {
        menusHandler.PositionPointer
            (
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].top,
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].bottom,
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].left,
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].right
            );
    }
}
