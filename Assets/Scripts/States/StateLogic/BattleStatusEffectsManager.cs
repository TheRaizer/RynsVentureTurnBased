using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleStatusEffectsManager
{
    private readonly BattleTextBoxHandler textBoxHandler;
    public StatusEffectAnimationState EffectAnimations { get; set; }

    public BattleStatusEffectsManager(BattleTextBoxHandler _textBoxHandler)
    {
        textBoxHandler = _textBoxHandler;
    }

    public bool CheckForStatusEffect(BattleHandler battleHandler, StatsManager currentInfectee)
    {
        bool addedMultiText = CheckForMultipleTurnStatusEffects(battleHandler);
        bool addSingleText = CheckForSingleTurnStatusEffects(battleHandler, currentInfectee);

        if(addedMultiText || addSingleText)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForReplacementStatusEffect(BattleHandler battleHandler, StatsManager currentInfectee, bool isAfterAttackChoice)
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
                if (statusEffect.runAfterAttackChoice != isAfterAttackChoice) return false;//return false if it is supposed to run after attack chosen but attack has not been chosen

                EffectAnimations.ReplacementEffect = statusEffect;
                textBoxHandler.PreviousState = null;
                textBoxHandler.AddTextAsStatusEffect(currentInfectee.user.Id, statusEffect.Name);
                EffectAnimations.ReplacementEffect = statusEffect;

                statusEffect.OnTurn(battleHandler, currentInfectee, battleHandler.BattleStateMachine, textBoxHandler);
                return true;
            }
        }
        else
        {
            return false;
        }
    }




    private bool CheckForMultipleTurnStatusEffects(BattleHandler battleHandler)
    {
        bool addedText = false;
        BattleEntitiesManager entitiesManager = battleHandler.BattleEntitiesManager;
        foreach (EntityType a in Enum.GetValues(typeof(EntityType)))//loop through both players and enemies
        {
            for (int i = 0; i < entitiesManager.AttackablesDic[a].Count; i++)//loop through the list of attackable entities
            {
                if (!entitiesManager.AttackablesDic[a][i].StatusEffectsManager.StatusEffectDicContainsKey(EffectType.MultiTurnTrigger)) continue;//skip if there is not effect type on current attackable
                StatsManager currentInfectee = entitiesManager.AttackablesDic[a][i];
                for (int j = 0; j < entitiesManager.AttackablesDic[a][i].StatusEffectsManager.GetStatusEffectListCount(EffectType.MultiTurnTrigger); j++)//loop through all the status effects of the current attackable entity
                {
                    StatusEffect currentStatusEffect = entitiesManager.AttackablesDic[a][i].StatusEffectsManager.GetStatusEffectFromList(EffectType.MultiTurnTrigger, j);
                    if (currentStatusEffect.HasEnded())
                    {
                        textBoxHandler.AddTextAsStatusEffectWornOff(entitiesManager.AttackablesDic[a][i].user.Id, currentStatusEffect.Name);
                        currentStatusEffect.OnWornOff(currentInfectee);
                        entitiesManager.AttackablesDic[a][i].StatusEffectsManager.RemoveFromStatusEffectsAtIndex(EffectType.MultiTurnTrigger, j);
                    }
                    else
                    {
                        EffectAnimations.StatusEffectsToAnimate.Add(currentStatusEffect);
                        if (currentStatusEffect.outputTextOnTurns)
                        {
                            textBoxHandler.AddTextAsStatusEffect(entitiesManager.AttackablesDic[a][i].user.Id, currentStatusEffect.Name);
                            addedText = true;
                        }

                        currentStatusEffect.OnTurn(battleHandler, entitiesManager.AttackablesDic[a][i], battleHandler.BattleStateMachine, textBoxHandler);
                    }
                }
            }
        }
        return addedText;
    }

    private bool CheckForSingleTurnStatusEffects(BattleHandler battleHandler, StatsManager currentInfectee)
    {
        bool addedText = false;
        for (int i = 0; i < currentInfectee.StatusEffectsManager.GetStatusEffectListCount(EffectType.SingleTurnTrigger); i++)
        {
            StatusEffect currentStatusEffect = currentInfectee.StatusEffectsManager.GetStatusEffectFromList(EffectType.SingleTurnTrigger, i).GetComponent<StatusEffect>();

            if (currentStatusEffect.HasEnded())
            {
                currentStatusEffect.OnWornOff(currentInfectee);
                textBoxHandler.AddTextAsStatusEffectWornOff(currentInfectee.user.Id, currentStatusEffect.Name);
                currentInfectee.StatusEffectsManager.RemoveFromStatusEffectsDic(EffectType.SingleTurnTrigger, currentStatusEffect, EffectAnimations);
            }
            else
            {
                EffectAnimations.StatusEffectsToAnimate.Add(currentStatusEffect);//should only run when status effect starts
                if (currentStatusEffect.outputTextOnTurns)
                {
                    textBoxHandler.AddTextAsStatusEffect(currentInfectee.user.Id, currentStatusEffect.Name);
                    addedText = true;
                }
                currentStatusEffect.OnTurn(battleHandler, currentInfectee, battleHandler.BattleStateMachine, textBoxHandler);
            }
        }
        return addedText;
    }
}
