using UnityEngine;

public class ItemPlayerChoiceState : PlayerChoiceState
{
    private readonly BattleHandler battleHandler;
    private readonly Inventory inventory;
    private readonly BattleTextBoxHandler textBox;
    private readonly BattleEntitiesManager battleEntitiesManager;

    public ItemPlayerChoiceState(StateMachine _stateMachine, BattleHandler _battleHandler, Inventory _inventory, BattleTextBoxHandler _textBox) : base(_stateMachine, _battleHandler.MenusHandler)
    {
        battleHandler = _battleHandler;
        inventory = _inventory;
        textBox = _textBox;
        battleEntitiesManager = battleHandler.BattleEntitiesManager;
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
            StatsManager userToHealStats = battleEntitiesManager.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats;
            if (userToHealStats.HealthManager.Dead && !battleHandler.ItemToUse.CanRevive())
            {
                textBox.PreviousState = BattleStates.ItemPlayerChoice;
                textBox.AddTextAsCannotRevive(battleHandler.ItemToUse.Id, battleEntitiesManager.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Id);
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
                StatsManager statsToUseOn = battleEntitiesManager.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats;
                inventory.UseItemInventoryInBattle(battleHandler.ItemIndex, statsToUseOn, null, stateMachine, textBox);
            }
        }
    }
}
