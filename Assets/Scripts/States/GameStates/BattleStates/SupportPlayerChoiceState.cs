using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class SupportPlayerChoiceState : PlayerChoiceState
{
    private readonly BattleLogic battleLogic;
    private readonly BattleTextBoxHandler textBoxHandler;

    public SupportPlayerChoiceState(StateMachine _stateMachine, BattleMenusHandler _battleMenusHandler, BattleLogic _battleLogic, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine, _battleMenusHandler)
    {
        battleLogic = _battleLogic;
        textBoxHandler = _textBoxHandler;
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            StatsManager playerToSupport = battleLogic.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats;

            if (battleLogic.CurrentPlayerAttack.ActionType == EntityAction.ActionTypes.Revive && !playerToSupport.HealthManager.Dead)
            {
                textBoxHandler.AddTextAsNonRevive(battleLogic.CurrentPlayer.Id, playerToSupport.user.Id);
                textBoxHandler.PreviousState = BattleStates.SupportPlayerChoice;
                stateMachine.ChangeState(BattleStates.BattleTextBox);
                return;
            }
            else if (!battleLogic.CurrentPlayer.Stats.ManaManager.CanUse(battleLogic.CurrentPlayerAttack.ManaReduction))
            {
                textBoxHandler.AddTextAsNotEnoughMana(battleLogic.CurrentPlayer.Id);
                textBoxHandler.PreviousState = BattleStates.SupportPlayerChoice;
                stateMachine.ChangeState(BattleStates.BattleTextBox);
                return;
            }
            EntityActionInfo actionInfo =  battleLogic.CurrentPlayerAttack.UseAction
                (
                    battleLogic.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats, battleLogic.CurrentPlayer.Stats.DamageScale, textBoxHandler
                );

            battleLogic.CheckForAttackablePlayers();
            battleLogic.textMods.PrintPlayerHealth();
            battleLogic.textMods.PrintPlayerIds();

            stateMachine.ChangeState(BattleStates.BattleTextBox);
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(BattleStates.MagicChoice);
        }
    }
}