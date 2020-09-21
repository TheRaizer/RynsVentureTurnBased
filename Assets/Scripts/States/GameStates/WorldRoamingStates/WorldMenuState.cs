using UnityEngine;

public class WorldMenuState : State
{
    protected readonly WorldMenusHandler worldMenusHandler;
    public WorldMenuState(StateMachine _stateMachine, WorldMenusHandler _worldMenusHandler) : base(_stateMachine)
    {
        worldMenusHandler = _worldMenusHandler;
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        worldMenusHandler.Traverse();

        if (Input.GetKeyDown(KeyCode.F))
        {
            stateMachine.ReturnBackToState(WorldRoamingStates.RoamState);
        }
    }
}
