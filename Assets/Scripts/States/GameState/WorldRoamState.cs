using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRoamState : State
{
    private readonly PlayerAnimations playerAnims;
    private readonly PlayerMovement playerMovement;

    public WorldRoamState(StateMachine _stateMachine, PlayerAnimations _playerAnims, PlayerMovement _playerMovement) : base(_stateMachine)
    {
        playerAnims = _playerAnims;
        playerMovement = _playerMovement;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        playerMovement.MovePlayer();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        playerAnims.DirectionAnim();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        if(Input.GetKeyDown(KeyCode.B))
        {
            stateMachine.ChangeState(typeof(BattleState));
        }
    }
}
