using UnityEngine;

public class InventoryState : WorldMenuState
{
    private readonly Inventory inventory;

    public InventoryState(StateMachine _stateMachine, WorldMenusHandler _worldMenusHandler, Inventory _inventory) : base(_stateMachine, _worldMenusHandler)
    {
        inventory = _inventory;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        worldMenusHandler.EmptyTextBoxes();
        worldMenusHandler.SetMenuTraversalMaxIndex(inventory.InventoryDic[inventory.CurrentInventoryOpen].Count - 1);

        inventory.PrintCurrentInventoryText(worldMenusHandler.TextBoxes);
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        CheckIfExitSelected();
    }

    private void CheckIfExitSelected()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(WorldRoamingStates.InventoryChoiceState);
        }
    }
}