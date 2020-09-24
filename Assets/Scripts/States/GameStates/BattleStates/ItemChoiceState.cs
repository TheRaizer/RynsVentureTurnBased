using UnityEngine;

public class ItemChoiceState : State
{
    private readonly BattleMenusHandler battleMenuHandler;
    private readonly Inventory inventory;
    private readonly BattleLogic battleLogic;
    private readonly VectorMenuTraversal itemTraversal;

    public ItemChoiceState(StateMachine _stateMachine, BattleMenusHandler _battleMenuHandler, Inventory _inventory, BattleLogic _battleLogic) : base(_stateMachine)
    {
        battleMenuHandler = _battleMenuHandler;
        inventory = _inventory;
        battleLogic = _battleLogic;
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
            battleLogic.ItemToUse = itemTraversal.currentIndex;
            Useable useable = (Useable)inventory.InventoryDic[inventory.CurrentInventoryOpen][battleLogic.ItemToUse];
            if (useable.UseOnAll())
            {
                //use on all
            }
            else if (!useable.UseOnAll())
            {
                //change state to pick a player to use item on
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
