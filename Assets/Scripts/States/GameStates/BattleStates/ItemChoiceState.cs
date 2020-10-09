using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ItemChoiceState : State
{
    private readonly BattleMenusHandler battleMenuHandler;
    private readonly Inventory inventory;
    private readonly BattleHandler battleHandler;
    private readonly VectorMenuTraversal itemTraversal;
    private readonly BattleTextBoxHandler textBoxHandler;

    public ItemChoiceState(StateMachine _stateMachine, BattleMenusHandler _battleMenuHandler, Inventory _inventory, BattleHandler _battleHandler, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleMenuHandler = _battleMenuHandler;
        inventory = _inventory;
        battleHandler = _battleHandler;
        textBoxHandler = _textBoxHandler;
        itemTraversal = new VectorMenuTraversal(PositionPointerForItemUse);
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        battleMenuHandler.ItemChoicePanel.SetActive(true);
        PositionPointerForItemUse();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        inventory.CurrentInventoryOpen = typeof(Useable);
        battleMenuHandler.EmptyItemTextBoxes();
        inventory.PrintCurrentInventoryText(battleMenuHandler.ItemTextBoxes);
        itemTraversal.MaxIndex = inventory.InventoryDic[inventory.CurrentInventoryOpen].Count - 1;
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        itemTraversal.Traverse();

        CheckIfEnterSelected();
        CheckIfExitSelected();
    }

    private void CheckIfExitSelected()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            stateMachine.ReturnBackToState(BattleStates.FightMenu);
        }
    }

    private void CheckIfEnterSelected()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            battleHandler.ItemIndex = itemTraversal.currentIndex;
            Useable useable = (Useable)inventory.InventoryDic[inventory.CurrentInventoryOpen][battleHandler.ItemIndex];
            battleHandler.ItemToUse = useable;

            if (useable.UseOnAll())
            {
                List<StatsManager> friendlyStats = new List<StatsManager>();
                foreach (PlayableCharacter p in battleHandler.ActivePlayableCharacters)
                {
                    friendlyStats.Add(p.Stats);
                }
                useable.OnUseInBattle(battleHandler.CurrentPlayer.Stats, friendlyStats, stateMachine, textBoxHandler);
            }
            else if (!useable.UseOnAll())
            {
                stateMachine.ChangeState(BattleStates.ItemPlayerChoice);
            }
        }
    }

    private void PositionPointerForItemUse()
    {
        battleMenuHandler.PositionPointer
            (
            battleMenuHandler.ItemUsePointerLocations[itemTraversal.currentIndex].top,
            battleMenuHandler.ItemUsePointerLocations[itemTraversal.currentIndex].bottom,
            battleMenuHandler.ItemUsePointerLocations[itemTraversal.currentIndex].left,
            battleMenuHandler.ItemUsePointerLocations[itemTraversal.currentIndex].right
        );
    }

    public override void OnExit()
    {
        base.OnExit();

        battleMenuHandler.ItemChoicePanel.SetActive(false);
    }
}
