using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WorldStates
{
    Roam,
    Battle
}

public class GameStateManager : MonoBehaviour
{
    [field: SerializeField] public List<GameObject> CharacterObjectRoster { get; private set; }

    public BattleMenusHandler BattleMenus { get; private set; }

    private StateMachine stateMachine;
    private BattleState battleState;
    private EnemyGenerator enemyGenerator;
    private Inventory inventory;
    private BattleEntitiesManager battleEntitiesManager;
    public WorldMenusHandler WorldMenus { get; private set; }

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        BattleMenus = GetComponent<BattleMenusHandler>();
        WorldMenus = GetComponent<WorldMenusHandler>();
        stateMachine = new StateMachine();
        battleState = new BattleState(stateMachine, BattleMenus, inventory);
        enemyGenerator = new EnemyGenerator(FindObjectOfType<EnemyStorageForArea>(), battleState.BattleHandler);
        battleState.EnemyGenerator = enemyGenerator;
        battleEntitiesManager = battleState.BattleHandler.BattleEntitiesManager;
        //FOR LOOP IS A TEST WAY TO ADD PLAYERS TO ACTIVE ROSTER
        for (int i = 0; i < CharacterObjectRoster.Count; i++)
        {
            battleEntitiesManager.ActivePlayableCharacters[i] = CharacterObjectRoster[i].GetComponent<PlayableCharacter>();
            battleEntitiesManager.PlayableCharacterRoster.Add(CharacterObjectRoster[i].GetComponent<PlayableCharacter>());
        }

        Dictionary<Enum, State> states = new Dictionary<Enum, State>()
        {
            { WorldStates.Roam, new WorldRoamingState(stateMachine, inventory, this) }
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
        CharacterObjectRoster.Add(playerObject);
        battleEntitiesManager.PlayableCharacterRoster.Add(playerObject.GetComponent<PlayableCharacter>());
    }
}
