using UnityEngine;

public class ItemPlayerChoiceState : PlayerChoiceState
{
    private readonly BattleLogic battleLogic;
    private readonly Inventory inventory;
    private readonly BattleTextBoxHandler textBox;

    public ItemPlayerChoiceState(StateMachine _stateMachine, BattleLogic _battleLogic, BattleMenusHandler _battleMenusHandler, Inventory _inventory, BattleTextBoxHandler _textBox) : base(_stateMachine, _battleMenusHandler)
    {
        battleLogic = _battleLogic;
        inventory = _inventory;
        textBox = _textBox;
    }

    public override void InputUpdate()
    {
        base.InputUpdate();


        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            StatsManager userToHealStats = battleLogic.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats;
            if (userToHealStats.HealthManager.Dead && !battleLogic.ItemToUse.CanRevive())
            {
                textBox.PreviousState = BattleStates.ItemPlayerChoice;
                textBox.AddTextAsNonRevive(battleLogic.ItemToUse.Id, battleLogic.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Id);
                stateMachine.ChangeState(BattleStates.BattleTextBox);
            }
            else if (battleLogic.ItemToUse.OnlyHeal() && userToHealStats.HealthManager.CurrentAmount == userToHealStats.HealthManager.MaxAmount)
            {
                textBox.PreviousState = BattleStates.ItemPlayerChoice;
                textBox.AddTextAsAlreadyMaxHealth(userToHealStats.user.Id);
                stateMachine.ChangeState(BattleStates.BattleTextBox);
            }
            else
            {
                inventory.UseItemInventoryInBattle(battleLogic.ItemIndex, battleLogic.ActivePlayableCharacters[vectorMenuTraversal.currentIndex].Stats, null, stateMachine, textBox);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(BattleStates.ItemChoice);
        }
    }
}
