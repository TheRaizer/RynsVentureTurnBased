using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WorldStates
{
    Roam,
    Battle
}

public class WorldStateManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterObjectRoster = null;

    public MenusHandler Menus { get; private set; }

    private StateMachine stateMachine;
    private BattleState battleState;
    private EnemyGenerator enemyGenerator;

    private void Awake()
    {
        Menus = GetComponent<MenusHandler>();
        stateMachine = new StateMachine();
        battleState = new BattleState(stateMachine, Menus);
        enemyGenerator = new EnemyGenerator(FindObjectOfType<EnemyStorageForArea>(), battleState.BattleLogic);
        battleState.EnemyGenerator = enemyGenerator;

        //FOR LOOP IS A TEST WAY TO ADD PLAYERS TO ACTIVE ROSTER
        for (int i = 0; i < characterObjectRoster.Count; i++)
        {
            battleState.BattleLogic.ActivePlayableCharacters[i] = characterObjectRoster[i].GetComponent<PlayableCharacter>();
            battleState.BattleLogic.PlayableCharacterRoster.Add(characterObjectRoster[i].GetComponent<PlayableCharacter>());
        }

        Dictionary<Enum, State> states = new Dictionary<Enum, State>()
        {
            { WorldStates.Roam, new WorldRoamState(stateMachine, characterObjectRoster[0].GetComponent<PlayerAnimations>(), characterObjectRoster[0].GetComponent<PlayerMovement>()) }
        };
        states.Add(WorldStates.Battle, battleState);


        stateMachine.Initialize(states, WorldStates.Roam);
    }

    private void Update()
    {
        stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        stateMachine.CurrentState.InputUpdate();
    }

    public void ChangePlayerObjects(GameObject playerObject)
    {
        characterObjectRoster.Add(playerObject);
        battleState.BattleLogic.PlayableCharacterRoster.Add(playerObject.GetComponent<PlayableCharacter>());
    }
}
