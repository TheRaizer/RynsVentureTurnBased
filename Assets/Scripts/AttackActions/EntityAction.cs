using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAction : MonoBehaviour
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

    private void ApplyStatusEffect(StatsManager statsToApplyToo, bool instantApply)
    {
        if (!instantApply)
        {
            float chance = UnityEngine.Random.Range(0, 100);

            if (chance <= StatusEffectChance)
            {
                StatusEffectApplication(statsToApplyToo);
            }
        }
        else
        {
            StatusEffectApplication(statsToApplyToo);
        }
    }

    private void StatusEffectApplication(StatsManager statsToApplyToo)
    {
        StatusEffect s = StatusEffectPrefab.GetComponent<StatusEffect>();

        s.OnEffectStart(statsToApplyToo);
        if(s.EffectType == EffectType.ReplaceTurn)
        {
            statsToApplyToo.StatusEffectsManager.AddToReplacementTurn(s.ShallowCopy());
        }
        else
        {
            statsToApplyToo.StatusEffectsManager.AddToStatusEffectsDic(s.EffectType, s.ShallowCopy());
        }
    }

    public List<EntityActionInfo> DetermineAttack(List<StatsManager> statsTooAttack, float damageScale, int indexToAttack)
    {
        List<EntityActionInfo> attackInfos = new List<EntityActionInfo>();
        if(!IsAOE)
        {
            attackInfos.Add(UseAttack(statsTooAttack[indexToAttack], damageScale));
        }
        else
        {
            for(int i = 0; i < statsTooAttack.Count; i++)
            {
                attackInfos.Add(UseAttack(statsTooAttack[i], damageScale));
            }
        }

        return attackInfos;
    }

    public EntityActionInfo UseAttack(StatsManager statsTooAttack, float damageScale)
    {
        int chance = UnityEngine.Random.Range(0, 100);
        WasCriticalHit = false;

        if (chance < Accuracy)
        {
            int critChance = UnityEngine.Random.Range(0, 100);

            if (critChance < CritChance)
            {
                statsTooAttack.HealthManager.Hit(MathExtension.RoundToNearestInteger(Damage * damageScale * CritMultiplier));
                if (StatusEffectPrefab != null)
                {
                    ApplyStatusEffect(statsTooAttack, true);
                }
                WasCriticalHit = true;
                Debug.Log("Critical hit");
                return new EntityActionInfo(statsTooAttack.user.Id, true);
            }

            statsTooAttack.HealthManager.Hit(MathExtension.RoundToNearestInteger(Damage * damageScale));
            if(StatusEffectPrefab != null)
            {
                ApplyStatusEffect(statsTooAttack, false);
            }
            return new EntityActionInfo(statsTooAttack.user.Id, true);
        }
        else
            return new EntityActionInfo(statsTooAttack.user.Id, false);
    }
}
