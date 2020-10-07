﻿using System;
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
    public BattleLogic BattleLogic { get; private set; }

    private readonly BattleStatusEffectsManager statusEffectsManager;
    private readonly BattleTextBoxHandler textBoxHandler;

    public BattleState(StateMachine _stateMachine, BattleMenusHandler _menusHandler, Inventory inventory) : base(_stateMachine)
    {
        menusHandler = _menusHandler;
        BattleStateMachine = new StateMachine();

        BattleLogic = new BattleLogic(menusHandler, BattleStateMachine);
        textBoxHandler = new BattleTextBoxHandler(menusHandler, BattleLogic, BattleStateMachine);
        statusEffectsManager = new BattleStatusEffectsManager(textBoxHandler, BattleStateMachine);

        Dictionary<Enum, State> battleStates = new Dictionary<Enum, State>()
        {
            { BattleStates.EnemyTurn, new EnemyTurnState(BattleStateMachine, BattleLogic, statusEffectsManager, textBoxHandler) },
            { BattleStates.Victory, new VictoryState(BattleStateMachine, this, menusHandler, BattleLogic, inventory) },
            { BattleStates.FightMenu, new FightMenuState(BattleStateMachine, BattleLogic, menusHandler, statusEffectsManager, textBoxHandler) },
            { BattleStates.BattleTextBox, new BattleTextBoxState(BattleStateMachine, textBoxHandler, menusHandler) },
            { BattleStates.MagicChoice, new MagicChoiceState(BattleStateMachine, BattleLogic, menusHandler, textBoxHandler) },
            { BattleStates.ItemChoice, new ItemChoiceState(BattleStateMachine, menusHandler, inventory, BattleLogic, textBoxHandler) },
            { BattleStates.ItemPlayerChoice, new ItemPlayerChoiceState(BattleStateMachine, BattleLogic, menusHandler, inventory, textBoxHandler) },
            { BattleStates.SupportPlayerChoice, new SupportPlayerChoiceState(BattleStateMachine, menusHandler, BattleLogic, textBoxHandler) },
            { BattleStates.EnemyChoice, new EnemyChoiceState(BattleStateMachine, BattleLogic, menusHandler, textBoxHandler, statusEffectsManager) },
            { BattleStates.AOEState, new AOEState(BattleStateMachine, BattleLogic, textBoxHandler) },
            { BattleStates.StatusEffectAnimations, new StatusEffectAnimationState(BattleStateMachine, BattleLogic, textBoxHandler) }
        };
        BattleStateMachine.Initialize(battleStates, BattleStates.FightMenu);
        statusEffectsManager.EffectAnimations = (StatusEffectAnimationState)BattleStateMachine.states[BattleStates.StatusEffectAnimations];
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        EnemyGenerator.InstantiateEnemies();

        BattleLogic.OutputActivePlayerSprites();
        BattleLogic.OutputEnemySprites();

        BattleLogic.ResetAllPlayerClockTicks();

        BattleLogic.TextMods.PrintEnemyIds();
        BattleLogic.TextMods.PrintPlayerIds();
        BattleLogic.TextMods.PrintPlayerHealth();
        BattleLogic.TextMods.PrintPlayerMana();

        menusHandler.BattleMenus.SetActive(true);
        BattleLogic.CheckForAttackablePlayers();
        BattleLogic.CheckForEnemiesRemaining();
        BattleLogic.CalculateNextTurn();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        BattleStateMachine.CurrentState.LogicUpdate();
        //Debug.Log(BattleStateMachine.CurrentState);
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
