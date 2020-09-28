using TMPro;
using UnityEngine;

public class InventoryChoiceState : WorldMenuState
{
    private readonly Inventory inventory;
    private int indexLeftOffAt = 0;

    public InventoryChoiceState(StateMachine _stateMachine, WorldMenusHandler _worldMenusHandler, Inventory _inventory) : base(_stateMachine, _worldMenusHandler)
    {
        inventory = _inventory;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        worldMenusHandler.EmptyTextBoxes();
        worldMenusHandler.SetMenuTraversalMaxIndex(inventory.InventoryDic.Count - 1);
        worldMenusHandler.SetMenuTraversalCurrentIndex(indexLeftOffAt);
        worldMenusHandler.PositionPointer();

        for (int i = 0; i < worldMenusHandler.InventoryChoiceOptions.Count; i++)
        {
            worldMenusHandler.TextBoxes[i].GetComponent<TextMeshProUGUI>().text = worldMenusHandler.InventoryChoiceOptions[i].OptionName;
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            indexLeftOffAt = worldMenusHandler.MenuTraversalCurrentIndex;
            worldMenusHandler.InventoryChoiceOptions[worldMenusHandler.MenuTraversalCurrentIndex].OnSelection(stateMachine, inventory);
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            indexLeftOffAt = 0;
            stateMachine.ReturnBackToState(WorldRoamingStates.MenuChoiceState);
        }
    }
}
