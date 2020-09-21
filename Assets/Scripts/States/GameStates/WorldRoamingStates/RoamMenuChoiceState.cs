using TMPro;
using UnityEngine;

public class RoamMenuChoiceState : WorldMenuState
{
    public RoamMenuChoiceState(StateMachine _stateMachine, WorldMenusHandler _worldMenusHandler) : base(_stateMachine, _worldMenusHandler)
    {
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        worldMenusHandler.MenuPanel.SetActive(true);
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        worldMenusHandler.EmptyTextBoxes();
        worldMenusHandler.SetMenuTraversalMaxIndex(worldMenusHandler.StartingMenuOptions.Count - 1);

        for(int i = 0; i < worldMenusHandler.StartingMenuOptions.Count; i++)
        {
            worldMenusHandler.TextBoxes[i].GetComponent<TextMeshProUGUI>().text = worldMenusHandler.StartingMenuOptions[i].OptionName;
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            worldMenusHandler.StartingMenuOptions[worldMenusHandler.MenuTraversalCurrentIndex].OnSelection(stateMachine);
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(WorldRoamingStates.RoamState);
        }
    }
}
