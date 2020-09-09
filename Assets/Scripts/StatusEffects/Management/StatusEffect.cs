using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffects
{
    None,
    Poison,
    Confusion,
    Sleep,
    Stun,
    Paralyses,
    AttackUp,
}

public enum EffectType
{
    MultiTurnTrigger,
    SingleTurnTrigger,
    ReplaceTurn
}

public class StatusEffect : MonoBehaviour
{
    [field: SerializeField] public StatusEffects AilmentType { get; private set; }
    [field: SerializeField] public EffectType EffectType { get; private set; }
    [field: SerializeField] public int MaxNumberOfTurnsToLast { get; private set; }
    [SerializeField] private int currentNumberOfTurnsToLast;

    private void Awake()
    {
        currentNumberOfTurnsToLast = MaxNumberOfTurnsToLast;
    }

    public virtual void OnEffectStart(StatsManager inhabitor) { }
    public virtual void OnTurn(List<StatsManager> attackableTeam, List<StatsManager> opposingTeam, StatsManager currentUser, BattleLogic battleLogic) => DecrementTurns();
    public virtual void OnWornOff(StatsManager inhabitor) { }

    public void DecrementTurns() => currentNumberOfTurnsToLast--;
    public void ResetNumberOfTurnsToLast() => currentNumberOfTurnsToLast = MaxNumberOfTurnsToLast;

    public bool HasEnded()
    {
        return currentNumberOfTurnsToLast <= 0;
    }
}