using System.Collections.Generic;
using UnityEngine;

public class EnemyChoiceState : State
{
    private readonly BattleLogic battleLogic;
    private readonly BattleMenusHandler menusHandler;
    public readonly VectorMenuTraversal menuTraversal;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleStatusEffectsManager battleStatusManager;

    public EnemyChoiceState(StateMachine _stateMachine, BattleLogic _battleLogic, BattleMenusHandler _menusHandler, BattleTextBoxHandler _textBoxHandler, BattleStatusEffectsManager _battleStatusManager) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        menusHandler = _menusHandler;
        textBoxHandler = _textBoxHandler;
        battleStatusManager = _battleStatusManager;

        menuTraversal = new VectorMenuTraversal(PositionPointerForEnemyChoice)
        {
            MaxIndex = battleLogic.Enemies.Length - 1
        };
    }

    
    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        while (battleLogic.Enemies[menuTraversal.currentIndex] == null)
        {
            menuTraversal.currentIndex++;
            menuTraversal.CheckIfIndexInRange();
        }
        PositionPointerForEnemyChoice();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        if (battleStatusManager.CheckForReplacementStatusEffect(battleLogic, battleLogic.CurrentPlayer.Stats, true))
        {
            return;
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        menuTraversal.TraverseWithNulls(battleLogic.Enemies);
        EnterChoice();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(BattleStates.FightMenu);
        }
    }

    private void EnterChoice()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E) && !battleLogic.AnimationsHandler.RanAnim)
        {
            Enemy enemyToAttack = battleLogic.Enemies[menuTraversal.currentIndex].GetComponent<Enemy>();
            EntityActionInfo attackInfo = battleLogic.CurrentPlayerAttack.UseAction(enemyToAttack.Stats, battleLogic.CurrentPlayer.Stats.DamageScale, textBoxHandler);

            List<GameObject> enemiesToAttack = new List<GameObject>
            {
                enemyToAttack.Animator.gameObject
            };
            battleLogic.SetActionPopupForEntity(battleLogic.CurrentPlayer.Animator.gameObject, enemiesToAttack, attackInfo.amount, attackInfo.CriticalHit, attackInfo.support, attackInfo.hitTarget);
            battleLogic.AnimationsHandler.RunAnim(battleLogic.CurrentPlayer.Stats.user.Animator, battleLogic.CurrentPlayerAttack.AnimToPlay, battleLogic.CurrentPlayerAttack.TriggerName);
            return;
        }
        if (battleLogic.AnimationsHandler.RanAnim)
        {
            if (!battleLogic.AnimationsHandler.IsRunningAnim())
            {
                Debug.Log("Move from animation to textbox");
                battleLogic.AnimationsHandler.RanAnim = false;
                battleLogic.CheckForEnemiesRemaining();
                battleLogic.TextMods.ChangeEnemyNameColour();
                battleLogic.BattleStateMachine.ChangeState(BattleStates.BattleTextBox);
            }
        }
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
