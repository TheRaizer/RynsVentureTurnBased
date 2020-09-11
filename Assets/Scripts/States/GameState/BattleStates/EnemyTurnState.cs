﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : StatusEffectCheckState
{
    private readonly BattleLogic battleLogic;
    private readonly TextModifications textMods;
    private readonly BattleStatusEffectsManager battleStatusManager;
    private readonly TextBoxHandler textBoxHandler;

    public EnemyTurnState(StateMachine _stateMachine, BattleLogic _battleLogic, TextModifications _textMods, BattleStatusEffectsManager _battleStatusManager, TextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        textMods = _textMods;
        battleStatusManager = _battleStatusManager;
        textBoxHandler = _textBoxHandler;
    }

    public override bool CheckedStatusEffectThisTurn { get; set; }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        Debug.Log(battleLogic.CurrentEnemy.Id + " Turn");
        if (CheckForStatusEffects(battleStatusManager, battleLogic, textBoxHandler, battleLogic.CurrentEnemy.Stats, null))
        {
            Debug.Log("printing status effects");
            stateMachine.ChangeState(BattleStates.BattleTextBox);
        }
        if (battleStatusManager.CheckForReplacementStatusEffect(battleLogic, battleLogic.CurrentEnemy.Stats, true))
        {
            return;
        }
        battleLogic.CheckForAttackablePlayers();

        int playerIndexToAttack = Random.Range(0, battleLogic.AttackablesDic[EntityType.Player].Count);
        EntityAction attackToUse = battleLogic.CurrentEnemy.Attacks[Random.Range(0, battleLogic.CurrentEnemy.Attacks.Count)];

        List<EntityActionInfo> attackInfos = attackToUse.DetermineAttack(battleLogic.AttackablesDic[EntityType.Player], battleLogic.CurrentEnemy.Stats.DamageScale, playerIndexToAttack);
        textBoxHandler.GenerateEnemyText(attackInfos, attackToUse);

        textMods.PrintPlayerHealth();
        textMods.ChangePlayerTextColour();
        Debug.Log("printing enemy attacks");
        stateMachine.ChangeState(BattleStates.BattleTextBox);
    }
}
