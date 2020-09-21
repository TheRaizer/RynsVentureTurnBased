using UnityEngine;

public class RoamState : State
{
    private readonly PlayerMovement playerToMove;
    private readonly PlayerAnimations playerToAnimate;
    private readonly WorldMenusHandler worldMenusHandler;

    public RoamState(StateMachine _stateMachine, PlayerMovement _playerToMove, PlayerAnimations _playerToAnimate, WorldMenusHandler _worldMenusHandler) : base(_stateMachine)
    {
        playerToMove = _playerToMove;
        playerToAnimate = _playerToAnimate;
        worldMenusHandler = _worldMenusHandler;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        worldMenusHandler.MenuPanel.SetActive(false);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        playerToMove.MovePlayer();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        playerToAnimate.DirectionAnim();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        if (Input.GetKeyDown(KeyCode.F))
        {
            stateMachine.ChangeState(WorldRoamingStates.MenuChoiceState);
        }
    }
}
