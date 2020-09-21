using TMPro;
using UnityEngine;

public class SettingsState : WorldMenuState
{
    public SettingsState(StateMachine _stateMachine, WorldMenusHandler _menusHandler) : base(_stateMachine, _menusHandler)
    {
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        worldMenusHandler.EmptyTextBoxes();
        worldMenusHandler.SetMenuTraversalMaxIndex(0);
        worldMenusHandler.TextBoxes[0].GetComponent<TextMeshProUGUI>().text = "This is the settings";
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(WorldRoamingStates.MenuChoiceState);
        }
    }
}
