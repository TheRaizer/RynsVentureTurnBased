using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : StatusEffectCheckState
{
    private readonly BattleLogic battleLogic;
    private readonly TextModifications textMods;
    private readonly BattleStatusEffectsManager statusManager;
    private readonly TextBoxHandler textBoxHandler;

    public EnemyTurnState(StateMachine _stateMachine, BattleLogic _battleLogic, TextModifications _textMods, BattleStatusEffectsManager _ailmentsManager, TextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        textMods = _textMods;
        statusManager = _ailmentsManager;
        textBoxHandler = _textBoxHandler;
    }

    public override bool CheckedStatusEffectThisTurn { get; set; }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        Debug.Log(battleLogic.CurrentEnemy.Id + " Turn");
        if (CheckForStatusEffects(statusManager, battleLogic, textBoxHandler, battleLogic.CurrentEnemy.Stats, null))
        {
            Debug.Log("printing status effects");
            stateMachine.ChangeState(BattleStates.BattleTextBox);
        }
        if (statusManager.CheckForReplacementStatusEffect(battleLogic.AttackablesDic, battleLogic.CurrentEnemy.Stats))
        {
            return;
        }
        battleLogic.CheckForAttackablePlayers();

        int playerIndexToAttack = Random.Range(0, battleLogic.AttackablesDic[EntityType.Player].Count);
        EntityAction attackToUse = battleLogic.CurrentEnemy.Attacks[Random.Range(0, battleLogic.CurrentEnemy.Attacks.Count)];

        List<EntityActionInfo> attackInfos = attackToUse.DetermineAttack(battleLogic.AttackablesDic[EntityType.Player], battleLogic.CurrentEnemy.Stats.DamageScale, playerIndexToAttack);
        GenerateText(attackInfos, attackToUse);

        textMods.PrintPlayerHealth();
        textMods.ChangePlayerTextColour();
        Debug.Log("printing enemy attacks");
        stateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    //probably move to battleLogic or smthn
    private void GenerateText(List<EntityActionInfo> attackInfos, EntityAction attackToUse)
    {
        for (int i = 0; i < attackInfos.Count; i++) 
        {
            textBoxHandler.AddTextAsAttack(battleLogic.CurrentEnemy.Id, attackToUse.AttackText, attackInfos[i].targetId);
            if (!attackInfos[i].hitTarget)
            {
                textBoxHandler.AddTextOnMiss(battleLogic.CurrentEnemy.Id, attackInfos[i].targetId);
            }
            else if(attackToUse.WasCriticalHit)
            {
                textBoxHandler.AddTextAsCriticalHit();
            }
        }
    }
}
