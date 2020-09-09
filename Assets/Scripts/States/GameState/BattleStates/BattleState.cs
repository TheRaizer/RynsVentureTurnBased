using System;
using UnityEngine;
using System.Collections.Generic;

public class BattleState : State
{
    private readonly MenusHandler menusHandler;
    public EnemyGenerator EnemyGenerator { private get; set; }
    public StateMachine BattleStateMachine { get; private set; }
    public BattleLogic BattleLogic { get; private set; }

    private readonly TextModifications textMods;
    private readonly StatusEffectsManager ailmentsManager;
    private readonly TextBoxHandler textBoxHandler;
    private readonly EnemyChoiceState enemyChoice;

    public BattleState(StateMachine _stateMachine, MenusHandler _menusHandler) : base(_stateMachine)
    {
        menusHandler = _menusHandler;
        BattleStateMachine = new StateMachine();

        BattleLogic = new BattleLogic(menusHandler, BattleStateMachine);
        textBoxHandler = new TextBoxHandler(menusHandler, BattleLogic, BattleStateMachine);
        ailmentsManager = new StatusEffectsManager(textBoxHandler, BattleLogic);

        textMods = new TextModifications(menusHandler, BattleLogic);

        Dictionary<Type, State> battleStates = new Dictionary<Type, State>()
        {
            { typeof(EnemyTurnState), new EnemyTurnState(BattleStateMachine, BattleLogic, textMods, ailmentsManager, textBoxHandler) },
            { typeof(VictoryState), new VictoryState(BattleStateMachine, stateMachine, menusHandler, BattleLogic) },
            { typeof(FightMenuState), new FightMenuState(BattleStateMachine, BattleLogic, menusHandler, ailmentsManager, textBoxHandler) },
            { typeof(BattleTextBoxState), new BattleTextBoxState(BattleStateMachine, textBoxHandler, menusHandler) }
        };
        enemyChoice = new EnemyChoiceState(BattleStateMachine, BattleLogic, menusHandler, textMods, textBoxHandler);
        battleStates.Add(typeof(EnemyChoiceState), enemyChoice);

        BattleStateMachine.Initialize(battleStates, typeof(FightMenuState));//probably start it off in the text box State EDDDIIIIIT THIIIIIIS
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        EnemyGenerator.InstantiateEnemies();
        BattleLogic.ResetAllPlayerClockTicks();
        textMods.PrintEnemyIds();
        textMods.PrintPlayerIds();
        textMods.PrintPlayerHealth();

        BattleLogic.CalculateNextTurn();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Debug.Log(BattleStateMachine.CurrentState);
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
    }
}
