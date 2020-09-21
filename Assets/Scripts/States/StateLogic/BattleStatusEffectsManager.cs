using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleStatusEffectsManager
{
    private readonly TextBoxHandler textBoxHandler;
    private readonly StateMachine battleStatemachine;

    public BattleStatusEffectsManager(TextBoxHandler _textBoxHandler, StateMachine _battleStatemachine)
    {
        textBoxHandler = _textBoxHandler;
        battleStatemachine = _battleStatemachine;
    }

    public bool CheckForStatusEffect(BattleLogic battleLogic, StatsManager currentInfectee)
    {
        bool addedMultiText = CheckForMultipleTurnStatusEffects(battleLogic);
        bool addSingleText = CheckForSingleTurnStatusEffects(battleLogic, currentInfectee);

        if(addedMultiText || addSingleText)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForReplacementStatusEffect(BattleLogic battleLogic, StatsManager currentInfectee, bool isAfterAttackChoice)
    {

        if (currentInfectee.StatusEffectsManager.GetStatusEffectListCount(EffectType.ReplaceTurn) > 0)
        {
            StatusEffect statusEffect = currentInfectee.StatusEffectsManager.GetStatusEffectFromList(EffectType.ReplaceTurn, 0);
            if (statusEffect.HasEnded())
            {
                textBoxHandler.AddTextAsStatusEffectWornOff(currentInfectee.user.Id, statusEffect.Name);
                currentInfectee.StatusEffectsManager.ClearStatusEffects(EffectType.ReplaceTurn);
                return false;
            }
            else
            {
                if (statusEffect.runAfterAttackChoice != isAfterAttackChoice) return false;

                textBoxHandler.PreviousState = null;
                textBoxHandler.AddTextAsStatusEffect(currentInfectee.user.Id, statusEffect.Name);
                if (currentInfectee.user.EntityType == EntityType.Enemy)
                {
                    statusEffect.OnTurn(battleLogic, currentInfectee, battleStatemachine, textBoxHandler);
                }
                else
                {
                    statusEffect.OnTurn(battleLogic, currentInfectee, battleStatemachine, textBoxHandler);
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }




    private bool CheckForMultipleTurnStatusEffects(BattleLogic battleLogic)
    {
        bool addedText = false;
        foreach (EntityType a in Enum.GetValues(typeof(EntityType)))//loop through both players and enemies
        {
            for (int i = 0; i < battleLogic.AttackablesDic[a].Count; i++)//loop through the list of players or enemies
            {
                if (!battleLogic.AttackablesDic[a][i].StatusEffectsManager.StatusEffectDicContainsKey(EffectType.MultiTurnTrigger)) continue;//skip if there is not effect type on current attackable

                for (int j = 0; j < battleLogic.AttackablesDic[a][i].StatusEffectsManager.GetStatusEffectListCount(EffectType.MultiTurnTrigger); j++)//loop through all the status effects of the current attackable
                {
                    StatusEffect currentStatusEffect = battleLogic.AttackablesDic[a][i].StatusEffectsManager.GetStatusEffectFromList(EffectType.MultiTurnTrigger, j);
                    if (currentStatusEffect.HasEnded())
                    {
                        textBoxHandler.AddTextAsStatusEffectWornOff(battleLogic.AttackablesDic[a][i].user.Id, currentStatusEffect.Name);
                        battleLogic.AttackablesDic[a][i].StatusEffectsManager.RemoveFromStatusEffectsAtIndex(EffectType.MultiTurnTrigger, j);
                        addedText = true;
                    }
                    else
                    {
                        if (currentStatusEffect.outputTextOnTurns)
                        {
                            textBoxHandler.AddTextAsStatusEffect(battleLogic.AttackablesDic[a][i].user.Id, currentStatusEffect.Name);
                            addedText = true;
                        }
                        if (a == EntityType.Player)
                        {
                            currentStatusEffect.OnTurn(battleLogic, battleLogic.AttackablesDic[a][i], battleStatemachine, textBoxHandler);
                        }
                        else if (a == EntityType.Enemy)
                        {
                            currentStatusEffect.OnTurn(battleLogic, battleLogic.AttackablesDic[a][i], battleStatemachine, textBoxHandler);
                        }
                    }
                }
            }
        }
        return addedText;
    }

    private bool CheckForSingleTurnStatusEffects(BattleLogic battleLogic, StatsManager currentInfectee)
    {
        bool addedText = false;
        for (int i = 0; i < currentInfectee.StatusEffectsManager.GetStatusEffectListCount(EffectType.SingleTurnTrigger); i++)
        {
            StatusEffect currentStatusEffect = currentInfectee.StatusEffectsManager.GetStatusEffectFromList(EffectType.SingleTurnTrigger, i).GetComponent<StatusEffect>();

            if (currentStatusEffect.HasEnded())
            {
                currentStatusEffect.OnWornOff(currentInfectee);
                textBoxHandler.AddTextAsStatusEffectWornOff(currentInfectee.user.Id, currentStatusEffect.Name);
                currentInfectee.StatusEffectsManager.RemoveFromStatusEffectsDic(EffectType.SingleTurnTrigger, currentStatusEffect);
                addedText = true;
            }
            else
            {
                if (currentStatusEffect.outputTextOnTurns)
                {
                    textBoxHandler.AddTextAsStatusEffect(currentInfectee.user.Id, currentStatusEffect.Name);
                    addedText = true;
                }
                switch (currentInfectee.user.EntityType)
                {
                    case EntityType.Enemy:
                        currentStatusEffect.OnTurn(battleLogic, currentInfectee, battleStatemachine, textBoxHandler);
                        break;
                    default:
                        currentStatusEffect.OnTurn(battleLogic, currentInfectee, battleStatemachine, textBoxHandler);
                        break;
                }
            }
        }
        return addedText;
    }
}
