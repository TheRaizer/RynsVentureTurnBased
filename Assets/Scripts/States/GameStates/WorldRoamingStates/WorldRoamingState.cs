using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WorldRoamingStates
{
     MenuChoiceState,
     InventoryChoiceState,
     InventoryState,
     RoamState,
     SettingsState,
}

public class WorldRoamingState : State
{
    private readonly GameStateManager gameStateManager;
    private readonly Inventory inventory;
    private readonly StateMachine worldRoamingStateMachine;

    public WorldRoamingState(StateMachine _stateMachine, Inventory _inventory, GameStateManager _gameStateManager) : base(_stateMachine)
    {
        gameStateManager = _gameStateManager;
        inventory = _inventory;

        PlayerMovement playerToMove = gameStateManager.CharacterObjectRoster[0].GetComponent<PlayerMovement>();
        PlayerAnimations playerToAnimate = gameStateManager.CharacterObjectRoster[0].GetComponent<PlayerAnimations>();

        worldRoamingStateMachine = new StateMachine();
        Dictionary<Enum, State> worldRoamStates = new Dictionary<Enum, State>()
        {
            { WorldRoamingStates.RoamState, new RoamState(worldRoamingStateMachine, playerToMove, playerToAnimate, gameStateManager.WorldMenus) },
            { WorldRoamingStates.MenuChoiceState, new RoamMenuChoiceState(worldRoamingStateMachine, gameStateManager.WorldMenus) },
            { WorldRoamingStates.InventoryChoiceState, new InventoryChoiceState(worldRoamingStateMachine, gameStateManager.WorldMenus, inventory) },
            { WorldRoamingStates.InventoryState, new InventoryState(worldRoamingStateMachine, gameStateManager.WorldMenus, inventory) },
            { WorldRoamingStates.SettingsState, new SettingsState(worldRoamingStateMachine, gameStateManager.WorldMenus) }
        };
        worldRoamingStateMachine.ConstantOnStateChange = gameStateManager.WorldMenus.ResetPointerPosition;
        worldRoamingStateMachine.Initialize(worldRoamStates, WorldRoamingStates.RoamState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        worldRoamingStateMachine.CurrentState.PhysicsUpdate();
        //Debug.Log(worldRoamingStateMachine.CurrentState);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        worldRoamingStateMachine.CurrentState.LogicUpdate();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
        worldRoamingStateMachine.CurrentState.InputUpdate();

        if (Input.GetKeyDown(KeyCode.B))
        {
            stateMachine.ChangeState(WorldStates.Battle);
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        gameStateManager.CharacterObjectRoster[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
