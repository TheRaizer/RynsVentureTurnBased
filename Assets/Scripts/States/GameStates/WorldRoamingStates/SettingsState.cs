using TMPro;
using UnityEngine;

public class SettingsState : WorldMenuState
{
    private int indexLeftOffAt = 0;
    public SettingsState(StateMachine _stateMachine, WorldMenusHandler _menusHandler) : base(_stateMachine, _menusHandler)
    {
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        worldMenusHandler.EmptyTextBoxes();
        worldMenusHandler.SetMenuTraversalMaxIndex(0);
        worldMenusHandler.TextBoxes[0].GetComponent<TextMeshProUGUI>().text = "This is the settings";
        worldMenusHandler.SetMenuTraversalCurrentIndex(indexLeftOffAt);
        worldMenusHandler.PositionPointer();
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
            indexLeftOffAt = worldMenusHandler.MenuTraversalCurrentIndex;
            stateMachine.ReturnBackToState(WorldRoamingStates.MenuChoiceState);
        }
    }
}
