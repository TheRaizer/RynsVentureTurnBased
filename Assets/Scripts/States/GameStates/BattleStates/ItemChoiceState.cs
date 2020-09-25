using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ItemChoiceState : State
{
    private readonly BattleMenusHandler battleMenuHandler;
    private readonly Inventory inventory;
    private readonly BattleLogic battleLogic;
    private readonly VectorMenuTraversal itemTraversal;
    private readonly BattleTextBoxHandler textBoxHandler;

    public ItemChoiceState(StateMachine _stateMachine, BattleMenusHandler _battleMenuHandler, Inventory _inventory, BattleLogic _battleLogic, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleMenuHandler = _battleMenuHandler;
        inventory = _inventory;
        battleLogic = _battleLogic;
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

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            battleLogic.ItemIndex = itemTraversal.currentIndex;
            Useable useable = (Useable)inventory.InventoryDic[inventory.CurrentInventoryOpen][battleLogic.ItemIndex];
            battleLogic.ItemToUse = useable;

            if (useable.UseOnAll())
            {
                List<StatsManager> friendlyStats = new List<StatsManager>();
                foreach(PlayableCharacter p in battleLogic.ActivePlayableCharacters)
                {
                    friendlyStats.Add(p.Stats);
                }
                useable.OnUseInBattle(battleLogic.CurrentPlayer.Stats, friendlyStats, stateMachine, textBoxHandler);
            }
            else if (!useable.UseOnAll())
            {
                stateMachine.ChangeState(BattleStates.ItemPlayerChoice);
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            stateMachine.ReturnBackToState(BattleStates.FightMenu);
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
