﻿using UnityEngine;

public class ItemPlayerChoiceState : State
{
    private readonly BattleLogic battleLogic;
    private readonly BattleMenusHandler battleMenusHandler;
    private readonly VectorMenuTraversal vectorMenuTraversal;
    private readonly Inventory inventory;
    private readonly BattleTextBoxHandler textBox;

    public ItemPlayerChoiceState(StateMachine _stateMachine, BattleLogic _battleLogic, BattleMenusHandler _battleMenusHandler, Inventory _inventory, BattleTextBoxHandler _textBox) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        battleMenusHandler = _battleMenusHandler;
        inventory = _inventory;
        textBox = _textBox;

        vectorMenuTraversal = new VectorMenuTraversal(PositionPointer)
        {
            MaxIndex = ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS - 1
        };
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        PositionPointer();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        vectorMenuTraversal.currentIndex = 0;
        PositionPointer();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        vectorMenuTraversal.Traverse();

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

    private void PositionPointer()
    {
        battleMenusHandler.PositionPointer
            (
                battleMenusHandler.ActivePlayerPointerLocation[vectorMenuTraversal.currentIndex].top,
                battleMenusHandler.ActivePlayerPointerLocation[vectorMenuTraversal.currentIndex].bottom,
                battleMenusHandler.ActivePlayerPointerLocation[vectorMenuTraversal.currentIndex].left,
                battleMenusHandler.ActivePlayerPointerLocation[vectorMenuTraversal.currentIndex].right
            );
    }
}
