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
    [field: SerializeField] public GameObject AnimatedVer { get; protected set; }
    [field: SerializeField] public AnimationClip AnimToPlay { get; protected set; }
    [field: SerializeField] public string TriggerName { get; protected set; }

    [SerializeField] private int currentNumberOfTurnsToLast;
    private GameObject parentObject; 

    public bool outputTextOnTurns = true;
    public bool runAfterAttackChoice = false;

    private void Awake()
    {
        currentNumberOfTurnsToLast = MaxNumberOfTurnsToLast;
    }

    public virtual void OnEffectStart(StatsManager inhabitor) 
    {
        if (AnimatedVer != null)
        {
            parentObject = GameObject.FindGameObjectWithTag("BattleEntities");
            GameObject g = Instantiate(AnimatedVer);
            g.transform.SetParent(parentObject.transform);
            AnimatedVer = g;
        }
    }
    public virtual void OnTurn(BattleHandler battleLogic, StatsManager infectee, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler) => DecrementTurns();
    public virtual void OnWornOff(StatsManager inhabitor) 
    {
        Destroy(AnimatedVer);
    }
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
