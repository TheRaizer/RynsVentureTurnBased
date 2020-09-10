using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleStatusEffectsManager
{
    private readonly TextBoxHandler textBoxHandler;
    private readonly BattleLogic battleLogic;

    public BattleStatusEffectsManager(TextBoxHandler _textBoxHandler, BattleLogic _battleLogic)
    {
        textBoxHandler = _textBoxHandler;
        battleLogic = _battleLogic;
    }

    public bool CheckForStatusEffect(Dictionary<EntityType, List<StatsManager>> attackablesDic, StatsManager currentInfectee)
    {
        bool addedMultiText = CheckForMultipleTurnStatusEffects(attackablesDic);
        bool addSingleText = CheckForSingleTurnStatusEffects(attackablesDic, currentInfectee);

        if(addedMultiText || addSingleText)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForReplacementStatusEffect(Dictionary<EntityType, List<StatsManager>> attackablesDic, StatsManager currentInfectee)
    {
        List<StatusEffect> currentStatusEffectsList = currentInfectee.StatusEffectsManager.GetEffectsList(EffectType.ReplaceTurn);

        if (currentStatusEffectsList != null && currentStatusEffectsList.Count > 0)
        {
            StatusEffect statusEffect = currentStatusEffectsList[0];
            if (currentStatusEffectsList[0].HasEnded())
            {
                textBoxHandler.AddTextAsStatusEffectWornOff(currentInfectee.user.Id, statusEffect.Name);
                currentInfectee.StatusEffectsManager.ClearStatusEffects(EffectType.ReplaceTurn);
                return false;
            }
            else
            {
                if (currentInfectee.user.EntityType == EntityType.Enemy)
                {
                    statusEffect.OnTurn(attackablesDic[currentInfectee.user.EntityType], attackablesDic[EntityType.Player], currentInfectee, battleLogic);
                }
                else
                {
                    statusEffect.OnTurn(attackablesDic[currentInfectee.user.EntityType], attackablesDic[EntityType.Enemy], currentInfectee, battleLogic);
                }
                textBoxHandler.AddTextAsStatusEffect(currentInfectee.user.Id, statusEffect.Name);
            }
            return true;
        }
        else
        {
            return false;
        }
    }




    private bool CheckForMultipleTurnStatusEffects(Dictionary<EntityType, List<StatsManager>> attackablesDic)
    {
        bool addedText = false;
        foreach (EntityType a in Enum.GetValues(typeof(EntityType)))//loop through both players and enemies
        {
            for (int i = 0; i < attackablesDic[a].Count; i++)//loop through the list of players or enemies
            {
                if (!attackablesDic[a][i].StatusEffectsManager.StatusEffectDicContainsKey(EffectType.MultiTurnTrigger)) continue;//skip if there is not effect type on current attackable

                List<StatusEffect> currentStatusEffectsList = attackablesDic[a][i].StatusEffectsManager.GetEffectsList(EffectType.MultiTurnTrigger);
                for (int j = 0; j < currentStatusEffectsList.Count; j++)//loop through all the status effects of the current attackable
                {

                    StatusEffect currentStatusEffect = currentStatusEffectsList[j];
                    if (currentStatusEffect.HasEnded())
                    {
                        textBoxHandler.AddTextAsStatusEffectWornOff(attackablesDic[a][i].user.Id, currentStatusEffect.Name);
                        currentStatusEffectsList.RemoveAt(j);
                        addedText = true;
                    }
                    else
                    {
                        if (a == EntityType.Player)
                        {
                            currentStatusEffect.OnTurn(attackablesDic[EntityType.Player], attackablesDic[EntityType.Enemy], attackablesDic[a][i], battleLogic);
                        }
                        else if (a == EntityType.Enemy)
                        {
                            currentStatusEffect.OnTurn(attackablesDic[EntityType.Enemy], attackablesDic[EntityType.Player], attackablesDic[a][i], battleLogic);
                        }
                        textBoxHandler.AddTextAsStatusEffect(attackablesDic[a][i].user.Id, currentStatusEffect.Name);
                        addedText = true;
                    }
                }
            }
        }
        return addedText;
    }

    private bool CheckForSingleTurnStatusEffects(Dictionary<EntityType, List<StatsManager>> attackablesDic, StatsManager currentInfectee)
    {
        bool addedText = false;
        List<StatusEffect> singleTurnList = currentInfectee.StatusEffectsManager.GetEffectsList(EffectType.SingleTurnTrigger);
        for (int i = 0; i < singleTurnList.Count; i++)
        {
            StatusEffect currentStatusEffect = singleTurnList[i].GetComponent<StatusEffect>();

            if (currentStatusEffect.HasEnded())
            {
                currentStatusEffect.OnWornOff(currentInfectee);
                textBoxHandler.AddTextAsStatusEffectWornOff(currentInfectee.user.Id, currentStatusEffect.Name);
                currentInfectee.StatusEffectsManager.RemoveFromStatusEffectsDic(EffectType.SingleTurnTrigger, currentStatusEffect);
                addedText = true;
            }
            else
            {
                if (currentInfectee.user.EntityType == EntityType.Enemy)
                {
                    currentStatusEffect.OnTurn(attackablesDic[currentInfectee.user.EntityType], attackablesDic[EntityType.Player], currentInfectee, battleLogic);
                }
                else
                {
                    currentStatusEffect.OnTurn(attackablesDic[currentInfectee.user.EntityType], attackablesDic[EntityType.Enemy], currentInfectee, battleLogic);
                }
                textBoxHandler.AddTextAsStatusEffect(currentInfectee.user.Id, currentStatusEffect.Name);
                addedText = true;
            }
        }
        return addedText;
    }
}
