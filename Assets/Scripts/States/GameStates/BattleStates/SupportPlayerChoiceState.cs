using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class SupportPlayerChoiceState : PlayerChoiceState
{
    private readonly BattleHandler battleHandler;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleEntitiesManager battleEntitiesManager;

    public SupportPlayerChoiceState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine, _battleHandler.MenusHandler)
    {
        battleHandler = _battleHandler;
        textBoxHandler = _textBoxHandler;
        battleEntitiesManager = battleHandler.BattleEntitiesManager;
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
            float damageScale = battleEntitiesManager.CurrentPlayer.Stats.DamageScale;
            battleEntitiesManager.CurrentPlayerAttack.UseAction
                (
                    battleEntitiesManager.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats, damageScale, textBoxHandler
                );

            battleEntitiesManager.CheckForAttackablePlayers();
            battleHandler.TextMods.PrintPlayerHealth();
            battleHandler.TextMods.PrintPlayerIds();

            stateMachine.ChangeState(BattleStates.BattleTextBox);
        }
    }

    private bool ManageSupportAction()
    {
        StatsManager playerToSupport = battleEntitiesManager.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats;

        if (battleEntitiesManager.CurrentPlayerAttack.ActionType == EntityAction.ActionTypes.Revive && !playerToSupport.HealthManager.Dead)
        {
            textBoxHandler.AddTextAsCannotRevive(battleEntitiesManager.CurrentPlayer.Id, playerToSupport.user.Id);
            textBoxHandler.PreviousState = BattleStates.SupportPlayerChoice;
            stateMachine.ChangeState(BattleStates.BattleTextBox);
            return true;
        }
        else if (!battleEntitiesManager.CurrentPlayer.Stats.ManaManager.CanUse(battleEntitiesManager.CurrentPlayerAttack.ManaReduction))
        {
            textBoxHandler.AddTextAsNotEnoughMana(battleEntitiesManager.CurrentPlayer.Id);
            textBoxHandler.PreviousState = BattleStates.SupportPlayerChoice;
            stateMachine.ChangeState(BattleStates.BattleTextBox);
            return true;
        }
        return false;
    }
}