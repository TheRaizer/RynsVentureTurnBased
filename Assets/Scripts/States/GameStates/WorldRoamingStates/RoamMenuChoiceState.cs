using TMPro;
using UnityEngine;

public class RoamMenuChoiceState : WorldMenuState
{
    private int indexLeftOffAt = 0;
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
        worldMenusHandler.SetMenuTraversalCurrentIndex(indexLeftOffAt);
        worldMenusHandler.PositionPointer();
        InitStartingMenuOptionsText();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        CheckIfEnterSelected();
        CheckIfExitSelected();
    }

    private void InitStartingMenuOptionsText()
    {
        for (int i = 0; i < worldMenusHandler.StartingMenuOptions.Count; i++)
        {
            worldMenusHandler.TextBoxes[i].GetComponent<TextMeshProUGUI>().text = worldMenusHandler.StartingMenuOptions[i].OptionName;
        }
    }

    private void CheckIfEnterSelected()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            indexLeftOffAt = worldMenusHandler.MenuTraversalCurrentIndex;
            worldMenusHandler.StartingMenuOptions[worldMenusHandler.MenuTraversalCurrentIndex].OnSelection(stateMachine);
        }
    }

    private void CheckIfExitSelected()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            indexLeftOffAt = 0;
            stateMachine.ReturnBackToState(WorldRoamingStates.RoamState);
        }
    }
}
