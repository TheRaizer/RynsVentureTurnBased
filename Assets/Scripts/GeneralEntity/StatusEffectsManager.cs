using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsManager
{
    private Dictionary<EffectType, List<StatusEffect>> statusEffectsDic { get; set; } = new Dictionary<EffectType, List<StatusEffect>>();
    private readonly StatsManager stats;

    public StatusEffectsManager(StatsManager _stats)
    {
        foreach (EffectType e in Enum.GetValues(typeof(EffectType)))
        {
            statusEffectsDic.Add(e, new List<StatusEffect>());
        }
        stats = _stats;
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

    public void PrintAllStatusEffects()
    {
        foreach (EffectType a in Enum.GetValues(typeof(EffectType)))
        {
            for(int i = 0; i < statusEffectsDic[a].Count; i++)
            {
                Debug.Log(stats.user.Id + " has status effect: " + statusEffectsDic[a][i] + " " + a);
            }
        }
    }

    public void AddToStatusEffectsDic(EffectType effectType, StatusEffect statusEffect)
    {
        if (statusEffectsDic.TryGetValue(effectType, out List<StatusEffect> effects))
        {
            if (statusEffectsDic[effectType].Contains(statusEffect))
            {
                statusEffectsDic[effectType][statusEffectsDic[effectType].IndexOf(statusEffect)].ResetNumberOfTurnsToLast();
            }
            else
            {
                statusEffectsDic[effectType].Add(statusEffect);
                statusEffect.OnEffectStart(stats);
            }
        }
        else
        {
            effects.Add(statusEffect);
            statusEffectsDic.Add(effectType, effects);
            statusEffect.OnEffectStart(stats);
        }
        PrintAllStatusEffects();
    }

    public void RemoveFromStatusEffectsDic(EffectType effectType, StatusEffect statusObject)
    {
        statusEffectsDic[effectType].Remove(statusObject);
    }

    public void AddToReplacementTurn(StatusEffect statusObject)
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
        statusObject.OnEffectStart(stats);
    }

    public void ClearStatusEffects(EffectType effectType)
    {
        statusEffectsDic[effectType].Clear();
    }

    public void RemoveFromStatusEffectsAtIndex(EffectType effectType, int index)
    {
        statusEffectsDic[effectType].RemoveAt(index);
    }

    public StatusEffect GetStatusEffectFromList(EffectType effectType, int index)
    {
        return statusEffectsDic[effectType][index];
    }

    public bool StatusEffectDicContainsKey(EffectType effectType)
    {
        return statusEffectsDic.ContainsKey(effectType);
    }

    public int GetStatusEffectListCount(EffectType effectType)
    {
        return statusEffectsDic[effectType].Count;
    }
}
