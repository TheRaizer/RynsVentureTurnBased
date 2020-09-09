using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public bool IsAOE { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float Accuracy { get; private set; }
    [field: SerializeField] public GameObject StatusEffectPrefab { get; private set; }
    [field: SerializeField] public float StatusEffectChance { get; private set; }
    [field: SerializeField] public float CritChance { get; private set; }
    [field: SerializeField] public float CritMultiplier { get; private set; }
    [field: SerializeField] public string AttackText { get; private set; }

    public bool WasCriticalHit { get; private set; } = false;

    private void ApplyStatusEffect(StatsManager stats, bool instantApply)
    {
        if (!instantApply)
        {
            float chance = UnityEngine.Random.Range(0, 100);

            if (chance <= StatusEffectChance)
            {
                StatusEffectApplication(stats);
            }
        }
        else
        {
            StatusEffectApplication(stats);
        }
    }

    private void StatusEffectApplication(StatsManager stats)
    {
        StatusEffect s = StatusEffectPrefab.GetComponent<StatusEffect>();
        switch (s.EffectType)
        {
            case EffectType.MultiTurnTrigger:
                if (stats.MultiTurnTriggeredAilments.Contains(StatusEffectPrefab))
                {
                    stats.MultiTurnTriggeredAilments[stats.MultiTurnTriggeredAilments.IndexOf(StatusEffectPrefab)].GetComponent<StatusEffect>().ResetNumberOfTurnsToLast();
                }
                else
                {
                    s.OnEffectStart(stats);
                    stats.MultiTurnTriggeredAilments.Add(Instantiate(StatusEffectPrefab));
                }
                break;

            case EffectType.ReplaceTurn:
                s.OnEffectStart(stats);
                if (stats.TurnReplaceAilment == StatusEffectPrefab)
                {
                    stats.TurnReplaceAilment.GetComponent<StatusEffect>().ResetNumberOfTurnsToLast();
                }
                else
                {
                    GameObject g = stats.TurnReplaceAilment;
                    stats.TurnReplaceAilment = Instantiate(StatusEffectPrefab);
                    Destroy(g);
                }
                break;

            case EffectType.SingleTurnTrigger:
                if (stats.SingleTurnTriggeredAilments.Contains(StatusEffectPrefab))
                {
                    stats.SingleTurnTriggeredAilments[stats.SingleTurnTriggeredAilments.IndexOf(StatusEffectPrefab)].GetComponent<StatusEffect>().ResetNumberOfTurnsToLast();
                }
                else
                {
                    s.OnEffectStart(stats);
                    stats.SingleTurnTriggeredAilments.Add(Instantiate(StatusEffectPrefab));
                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public List<AttackInfo> DetermineAttack(List<StatsManager> stats, float damageScale, int indexToAttack)
    {
        List<AttackInfo> attackInfos = new List<AttackInfo>();
        if(!IsAOE)
        {
            attackInfos.Add(UseAttack(stats[indexToAttack], damageScale));
        }
        else
        {
            for(int i = 0; i < stats.Count; i++)
            {
                attackInfos.Add(UseAttack(stats[i], damageScale));
            }
        }

        return attackInfos;
    }

    public AttackInfo UseAttack(StatsManager stats, float damageScale)
    {
        int chance = UnityEngine.Random.Range(0, 100);
        WasCriticalHit = false;

        if (chance < Accuracy)
        {
            int critChance = UnityEngine.Random.Range(0, 100);

            if (critChance < CritChance)
            {
                stats.HealthManager.Hit(MathExtension.RoundToNearestInteger(Damage * damageScale * CritMultiplier));
                if (StatusEffectPrefab != null)
                {
                    ApplyStatusEffect(stats, true);
                }
                WasCriticalHit = true;
                Debug.Log("Critical hit");
                return new AttackInfo(stats.user.Id, true);
            }

            stats.HealthManager.Hit(MathExtension.RoundToNearestInteger(Damage * damageScale));

            if(StatusEffectPrefab != null)
            {
                ApplyStatusEffect(stats, false);
            }
            return new AttackInfo(stats.user.Id, true);
        }
        else
            return new AttackInfo(stats.user.Id, false);
    }
}
