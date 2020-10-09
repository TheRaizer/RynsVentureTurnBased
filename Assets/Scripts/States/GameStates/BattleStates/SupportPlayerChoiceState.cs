using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class SupportPlayerChoiceState : PlayerChoiceState
{
    private readonly BattleHandler battleHandler;
    private readonly BattleTextBoxHandler textBoxHandler;

    public SupportPlayerChoiceState(StateMachine _stateMachine, BattleMenusHandler _battleMenusHandler, BattleHandler _battleHandler, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine, _battleMenusHandler)
    {
        battleHandler = _battleHandler;
        textBoxHandler = _textBoxHandler;
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        OnSelection();
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(BattleStates.MagicChoice);
        }
    }

    private void OnSelection()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            if (ManageSupportAction()) return;
            battleHandler.CurrentPlayerAttack.UseAction
                (
                    battleHandler.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats, battleHandler.CurrentPlayer.Stats.DamageScale, textBoxHandler
                );

            battleHandler.CheckForAttackablePlayers();
            battleHandler.TextMods.PrintPlayerHealth();
            battleHandler.TextMods.PrintPlayerIds();

            stateMachine.ChangeState(BattleStates.BattleTextBox);
        }
    }

    private bool ManageSupportAction()
    {
        StatsManager playerToSupport = battleHandler.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats;

        if (battleHandler.CurrentPlayerAttack.ActionType == EntityAction.ActionTypes.Revive && !playerToSupport.HealthManager.Dead)
        {
            textBoxHandler.AddTextAsCannotRevive(battleHandler.CurrentPlayer.Id, playerToSupport.user.Id);
            textBoxHandler.PreviousState = BattleStates.SupportPlayerChoice;
            stateMachine.ChangeState(BattleStates.BattleTextBox);
            return true;
        }
        else if (!battleHandler.CurrentPlayer.Stats.ManaManager.CanUse(battleHandler.CurrentPlayerAttack.ManaReduction))
        {
            textBoxHandler.AddTextAsNotEnoughMana(battleHandler.CurrentPlayer.Id);
            textBoxHandler.PreviousState = BattleStates.SupportPlayerChoice;
            stateMachine.ChangeState(BattleStates.BattleTextBox);
            return true;
        }
        return false;
    }
}