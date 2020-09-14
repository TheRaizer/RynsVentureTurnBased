using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsManager
{
    private readonly Dictionary<EffectType, List<StatusEffect>> statusEffectsDic = new Dictionary<EffectType, List<StatusEffect>>();
    private readonly StatsManager stats;

    public StatusEffectsManager(StatsManager _stats)
    {
        foreach (EffectType e in Enum.GetValues(typeof(EffectType)))
        {
            statusEffectsDic.Add(e, new List<StatusEffect>());
        }
        stats = _stats;
    }

    public void PrintStatusEffects()
    {
        foreach (EffectType e in Enum.GetValues(typeof(EffectType)))
        {
            foreach (StatusEffect s in statusEffectsDic[e])
            {
                Debug.Log(s.Name);
            }
        }
    }

    public void RemoveAllStatusEffects()
    {
        foreach (EffectType e in Enum.GetValues(typeof(EffectType)))
        {
            foreach (StatusEffect s in statusEffectsDic[e])
            {
                s.OnWornOff(stats);
            }
            statusEffectsDic[e].Clear();
        }
    }

    public void AddToStatusEffectsDic(EffectType effectType, StatusEffect statusObject, TextBoxHandler textBoxHandler)
    {
        if (statusEffectsDic.TryGetValue(effectType, out List<StatusEffect> effects))
        {
            if (statusEffectsDic[effectType].Contains(statusObject))
            {
                statusEffectsDic[effectType][statusEffectsDic[effectType].IndexOf(statusObject)].ResetNumberOfTurnsToLast();
            }
            else
            {
                statusEffectsDic[effectType].Add(statusObject);
                statusObject.OnEffectStart(stats, textBoxHandler);
            }
        }
        else
        {
            effects.Add(statusObject);
            statusEffectsDic.Add(effectType, effects);
            statusObject.OnEffectStart(stats, textBoxHandler);
        }
    }

    public void RemoveFromStatusEffectsDic(EffectType effectType, StatusEffect statusObject)
    {
        statusEffectsDic[effectType].Remove(statusObject);
    }

    public void AddToReplacementTurn(StatusEffect statusObject, TextBoxHandler textBoxHandler)
    {
        if (statusEffectsDic.TryGetValue(EffectType.ReplaceTurn, out List<StatusEffect> effects))
        {
            statusEffectsDic[EffectType.ReplaceTurn].Clear();
            statusEffectsDic[EffectType.ReplaceTurn].Add(statusObject);
        }
        else
        {
            effects.Add(statusObject);
            statusEffectsDic.Add(EffectType.ReplaceTurn, effects);
        }
        statusObject.OnEffectStart(stats, textBoxHandler);
    }

    public void ClearStatusEffects(EffectType effectType)
    {
        statusEffectsDic[effectType].Clear();
    }

    public List<StatusEffect> GetEffectsList(EffectType effectType)
    {
        return statusEffectsDic[effectType];
    }

    public bool StatusEffectDicContainsKey(EffectType effectType)
    {
        return statusEffectsDic.ContainsKey(effectType);
    }
}
