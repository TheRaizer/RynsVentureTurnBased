using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : StatusEffectCheckState
{
    private readonly BattleLogic battleLogic;
    private readonly TextModifications textMods;
    private readonly StatusEffectsManager ailmentsManager;
    private readonly TextBoxHandler textBoxHandler;

    public EnemyTurnState(StateMachine _stateMachine, BattleLogic _battleLogic, TextModifications _textMods, StatusEffectsManager _ailmentsManager, TextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        textMods = _textMods;
        ailmentsManager = _ailmentsManager;
        textBoxHandler = _textBoxHandler;
    }

    public override bool CheckedStatusEffectThisTurn { get; set; }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        if (CheckForStatusEffects(ailmentsManager, battleLogic, textBoxHandler, battleLogic.CurrentEnemy.Stats, this))
        {
            Debug.Log("Checked status effects");
            stateMachine.ChangeState(typeof(BattleTextBoxState));
        }
        battleLogic.CheckForAttackablePlayers();
        Debug.Log("Enemy Turn");

        int playerIndexToAttack = Random.Range(0, battleLogic.attackablePlayers.Count);
        Attack attackToUse = battleLogic.CurrentEnemy.Attacks[Random.Range(0, battleLogic.CurrentEnemy.Attacks.Count)];

        List<AttackInfo> attackInfos = attackToUse.DetermineAttack(battleLogic.attackablePlayers, battleLogic.CurrentEnemy.Stats.DamageScale, playerIndexToAttack);
        GenerateText(attackInfos, attackToUse);

        textMods.PrintPlayerHealth();
        textMods.ChangePlayerTextColour();

        stateMachine.ChangeState(typeof(BattleTextBoxState));
    }

    //probably move to battleLogic or smthn
    private void GenerateText(List<AttackInfo> attackInfos, Attack attackToUse)
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
