using System;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    MultiTurnTrigger,
    SingleTurnTrigger,
    ReplaceTurn,
}

public class StatusEffect : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public EffectType EffectType { get; private set; }
    [field: SerializeField] public int MaxNumberOfTurnsToLast { get; private set; }
    [SerializeField] private int currentNumberOfTurnsToLast;
    [SerializeField] public bool outputTextOnTurns = true;
    [SerializeField] public bool runAfterAttackChoice = false;

    private void Awake()
    {
        currentNumberOfTurnsToLast = MaxNumberOfTurnsToLast;
    }

    public virtual void OnEffectStart(StatsManager inhabitor, TextBoxHandler textBoxHandler) { }
    public virtual void OnTurn(BattleLogic battleLogic, StatsManager currentUser, StateMachine battleStateMachine, TextBoxHandler textBoxHandler) => DecrementTurns();
    public virtual void OnWornOff(StatsManager inhabitor) { }

    public void DecrementTurns() => currentNumberOfTurnsToLast--;
    public void ResetNumberOfTurnsToLast() => currentNumberOfTurnsToLast = MaxNumberOfTurnsToLast;

    public virtual StatusEffect ShallowCopy()//shallow copy means that data types are cloned but reference types are just referenced from the original
    {
        return (StatusEffect)MemberwiseClone();
    }

    public bool HasEnded()
    {
        return currentNumberOfTurnsToLast <= 0;
    }
}

public interface ICloneable<T>
{
    T ShallowCopy();
}