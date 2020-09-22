using System;
using UnityEngine;
using System.Collections.Generic;

public enum BattleStates
{
    EnemyTurn,
    FightMenu,
    EnemyChoice,
    BattleTextBox,
    Victory,
    Loss,
    MagicChoice,
}

public class BattleState : State
{
    private readonly BattleMenusHandler menusHandler;
    private readonly StateMachine BattleStateMachine;

    public EnemyGenerator EnemyGenerator { private get; set; }
    public BattleLogic BattleLogic { get; private set; }

    private readonly TextModifications textMods;
    private readonly BattleStatusEffectsManager statusEffectsManager;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly EnemyChoiceState enemyChoice;

    public BattleState(StateMachine _stateMachine, BattleMenusHandler _menusHandler) : base(_stateMachine)
    {
        menusHandler = _menusHandler;
        BattleStateMachine = new StateMachine();

        BattleLogic = new BattleLogic(menusHandler, BattleStateMachine);
        textBoxHandler = new BattleTextBoxHandler(menusHandler, BattleLogic, BattleStateMachine);
        statusEffectsManager = new BattleStatusEffectsManager(textBoxHandler, BattleStateMachine);

        textMods = new TextModifications(menusHandler, BattleLogic);

        Dictionary<Enum, State> battleStates = new Dictionary<Enum, State>()
        {
            { BattleStates.EnemyTurn, new EnemyTurnState(BattleStateMachine, BattleLogic, textMods, statusEffectsManager, textBoxHandler) },
            { BattleStates.Victory, new VictoryState(BattleStateMachine, this, menusHandler, BattleLogic) },
            { BattleStates.FightMenu, new FightMenuState(BattleStateMachine, BattleLogic, menusHandler, statusEffectsManager, textBoxHandler) },
            { BattleStates.BattleTextBox, new BattleTextBoxState(BattleStateMachine, textBoxHandler, menusHandler) },
            { BattleStates.MagicChoice, new MagicChoiceState(BattleStateMachine, BattleLogic, menusHandler) }
        };
        enemyChoice = new EnemyChoiceState(BattleStateMachine, BattleLogic, menusHandler, textMods, textBoxHandler, statusEffectsManager);
        battleStates.Add(BattleStates.EnemyChoice, enemyChoice);

        BattleStateMachine.Initialize(battleStates, BattleStates.FightMenu);//probably start it off in the text box State EDDDIIIIIT THIIIIIIS
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        EnemyGenerator.InstantiateEnemies();
        BattleLogic.ResetAllPlayerClockTicks();
        textMods.PrintEnemyIds();
        textMods.PrintPlayerIds();
        textMods.PrintPlayerHealth();

        menusHandler.BattleMenus.SetActive(true);
        BattleLogic.CheckForAttackablePlayers();
        BattleLogic.CheckForEnemiesRemaining();
        BattleLogic.CalculateNextTurn();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        BattleStateMachine.CurrentState.LogicUpdate();
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

        enemyChoice.menuTraversal.ResetCurrentIndex();
        menusHandler.BattleMenus.SetActive(false);
    }

    public void ChangeToWorldRoamState()
    {
        stateMachine.ChangeState(WorldStates.Roam);
    }
}
