using System;
using System.Collections.Generic;
using UnityEngine;

public enum BattleStates
{
    EnemyTurn,
    FightMenu,
    EnemyChoice,
    BattleTextBox,
    Victory,
    Loss,
    MagicChoice,
    ItemChoice,
    ItemPlayerChoice,
    SupportPlayerChoice,
    AOEState,
    StatusEffectAnimations,
}

public class BattleState : State
{
    private readonly BattleMenusHandler menusHandler;
    private readonly StateMachine BattleStateMachine;

    public EnemyGenerator EnemyGenerator { private get; set; }
    public BattleHandler BattleHandler { get; private set; }

    private readonly BattleStatusEffectsManager statusEffectsManager;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleEntitiesManager battleEntitiesManager;

    public BattleState(StateMachine _stateMachine, BattleMenusHandler _menusHandler, Inventory inventory) : base(_stateMachine)
    {
        menusHandler = _menusHandler;
        BattleStateMachine = new StateMachine();

        BattleHandler = new BattleHandler(menusHandler, BattleStateMachine);
        battleEntitiesManager = BattleHandler.BattleEntitiesManager;
        textBoxHandler = new BattleTextBoxHandler(menusHandler, BattleHandler, BattleStateMachine);
        statusEffectsManager = new BattleStatusEffectsManager(textBoxHandler);

        Dictionary<Enum, State> battleStates = new Dictionary<Enum, State>()
        {
            { BattleStates.EnemyTurn, new EnemyTurnState(BattleStateMachine, BattleHandler, statusEffectsManager, textBoxHandler) },
            { BattleStates.Victory, new VictoryState(BattleStateMachine, this, BattleHandler, inventory) },
            { BattleStates.FightMenu, new FightMenuState(BattleStateMachine, BattleHandler, statusEffectsManager, textBoxHandler) },
            { BattleStates.BattleTextBox, new BattleTextBoxState(BattleStateMachine, textBoxHandler, BattleHandler) },
            { BattleStates.MagicChoice, new MagicChoiceState(BattleStateMachine, BattleHandler, textBoxHandler) },
            { BattleStates.ItemChoice, new ItemChoiceState(BattleStateMachine, inventory, BattleHandler, textBoxHandler) },
            { BattleStates.ItemPlayerChoice, new ItemPlayerChoiceState(BattleStateMachine, BattleHandler, inventory, textBoxHandler) },
            { BattleStates.SupportPlayerChoice, new SupportPlayerChoiceState(BattleStateMachine, BattleHandler, textBoxHandler) },
            { BattleStates.EnemyChoice, new EnemyChoiceState(BattleStateMachine, BattleHandler, textBoxHandler, statusEffectsManager) },
            { BattleStates.AOEState, new AOEState(BattleStateMachine, BattleHandler, textBoxHandler) },
            { BattleStates.StatusEffectAnimations, new StatusEffectAnimationState(BattleStateMachine, BattleHandler, textBoxHandler) }
        };
        BattleStateMachine.Initialize(battleStates, BattleStates.FightMenu);
        statusEffectsManager.EffectAnimations = (StatusEffectAnimationState)BattleStateMachine.states[BattleStates.StatusEffectAnimations];
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        EnemyGenerator.InstantiateEnemies();

        BattleHandler.BattleEntitySprites.OutputActivePlayerSprites();
        BattleHandler.BattleEntitySprites.OutputEnemySprites();

        battleEntitiesManager.ResetAllPlayerClockTicks();

        BattleHandler.TextMods.PrintEnemyIds();
        BattleHandler.TextMods.PrintPlayerIds();
        BattleHandler.TextMods.PrintPlayerHealth();
        BattleHandler.TextMods.PrintPlayerMana();

        menusHandler.BattleMenus.SetActive(true);
        battleEntitiesManager.CheckForAttackablePlayers();
        battleEntitiesManager.CheckForEnemiesRemaining();
        battleEntitiesManager.CalculateNextTurn();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        BattleStateMachine.CurrentState.LogicUpdate();
        Debug.Log(BattleStateMachine.CurrentState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        BattleStateMachine.CurrentState.PhysicsUpdate();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        BattleStateMachine.CurrentState.InputUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        EnemyChoiceState enemyChoiceState = (EnemyChoiceState)BattleStateMachine.states[BattleStates.EnemyChoice];
        enemyChoiceState.menuTraversal.ResetCurrentIndex();
        menusHandler.BattleMenus.SetActive(false);
    }

    public void ChangeToWorldRoamState()
    {
        stateMachine.ChangeState(WorldStates.Roam);
    }
}
