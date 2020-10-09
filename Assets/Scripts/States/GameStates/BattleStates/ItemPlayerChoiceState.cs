using UnityEngine;

public class ItemPlayerChoiceState : PlayerChoiceState
{
    private readonly BattleHandler battleHandler;
    private readonly Inventory inventory;
    private readonly BattleTextBoxHandler textBox;

    public ItemPlayerChoiceState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleMenusHandler _battleMenusHandler, Inventory _inventory, BattleTextBoxHandler _textBox) : base(_stateMachine, _battleMenusHandler)
    {
        battleHandler = _battleHandler;
        inventory = _inventory;
        textBox = _textBox;
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        CheckIfEnterSelected();
        CheckIfExitSelected();
    }

    private void CheckIfExitSelected()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(BattleStates.ItemChoice);
        }
    }

    private void CheckIfEnterSelected()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            StatsManager userToHealStats = battleHandler.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats;
            if (userToHealStats.HealthManager.Dead && !battleHandler.ItemToUse.CanRevive())
            {
                textBox.PreviousState = BattleStates.ItemPlayerChoice;
                textBox.AddTextAsCannotRevive(battleHandler.ItemToUse.Id, battleHandler.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Id);
                stateMachine.ChangeState(BattleStates.BattleTextBox);
            }
            else if (battleHandler.ItemToUse.OnlyHeal() && userToHealStats.HealthManager.CurrentAmount == userToHealStats.HealthManager.MaxAmount)
            {
                textBox.PreviousState = BattleStates.ItemPlayerChoice;
                textBox.AddTextAsAlreadyMaxHealth(userToHealStats.user.Id);
                stateMachine.ChangeState(BattleStates.BattleTextBox);
            }
            else
            {
                inventory.UseItemInventoryInBattle(battleHandler.ItemIndex, battleHandler.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats, null, stateMachine, textBox);
            }
        }
    }
}
