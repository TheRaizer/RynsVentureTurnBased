using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAction : MonoBehaviour
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public bool IsAOE { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int ManaReduction { get; private set; }
    [field: SerializeField] public float Accuracy { get; private set; }
    [field: SerializeField] public GameObject StatusEffectPrefab { get; private set; }
    [field: SerializeField] public float StatusEffectChance { get; private set; }
    [field: SerializeField] public float CritChance { get; private set; }
    [field: SerializeField] public float CritMultiplier { get; private set; }
    [field: SerializeField] public string AttackText { get; private set; }

    public bool WasCriticalHit { get; private set; } = false;

    private void ApplyStatusEffect(StatsManager statsToApplyToo, bool instantApply, BattleTextBoxHandler textBoxHandler)
    {
        if (!instantApply)
        {
            float chance = UnityEngine.Random.Range(0, 100);

            if (chance <= StatusEffectChance)
            {
                StatusEffectApplication(statsToApplyToo, textBoxHandler);
            }
        }
        else
        {
            StatusEffectApplication(statsToApplyToo, textBoxHandler);
        }
    }

    private void StatusEffectApplication(StatsManager statsToApplyToo, BattleTextBoxHandler textBoxHandler)
    {
        StatusEffect s = StatusEffectPrefab.GetComponent<StatusEffect>();
        Debug.Log("Apply " + s.Name + " too " + statsToApplyToo.user.Id);
        if(s.EffectType == EffectType.ReplaceTurn)
        {
            statsToApplyToo.StatusEffectsManager.AddToReplacementTurn(s.ShallowCopy(), textBoxHandler);
        }
        else
        {
            statsToApplyToo.StatusEffectsManager.AddToStatusEffectsDic(s.EffectType, s.ShallowCopy(), textBoxHandler);
        }
    }

    public List<EntityActionInfo> DetermineAttack(List<StatsManager> statsTooAttack, float damageScale, int indexToAttack, BattleTextBoxHandler textBoxHandler)
    {
        List<EntityActionInfo> attackInfos = new List<EntityActionInfo>();
        if(!IsAOE)
        {
            attackInfos.Add(UseAttack(statsTooAttack[indexToAttack], damageScale, textBoxHandler));
        }
        else
        {
            for(int i = 0; i < statsTooAttack.Count; i++)
            {
                attackInfos.Add(UseAttack(statsTooAttack[i], damageScale, textBoxHandler));
            }
        }

        return attackInfos;
    }

    public EntityActionInfo UseAttack(StatsManager statsTooAttack, float damageScale, BattleTextBoxHandler textBoxHandler)
    {
        int chance = UnityEngine.Random.Range(0, 100);
        WasCriticalHit = false;

        if (chance < Accuracy)
        {
            int critChance = UnityEngine.Random.Range(0, 100);

            if (critChance < CritChance)
            {
                statsTooAttack.HealthManager.ReduceAmount(MathExtension.RoundToNearestInteger(Damage * damageScale * CritMultiplier));
                if (StatusEffectPrefab != null)
                {
                    ApplyStatusEffect(statsTooAttack, true, textBoxHandler);
                }
                WasCriticalHit = true;
                Debug.Log("Critical hit");
                return new EntityActionInfo(statsTooAttack.user.Id, true);
            }

            statsTooAttack.HealthManager.ReduceAmount(MathExtension.RoundToNearestInteger(Damage * damageScale));
            if(StatusEffectPrefab != null)
            {
                ApplyStatusEffect(statsTooAttack, false, textBoxHandler);
            }
            return new EntityActionInfo(statsTooAttack.user.Id, true);
        }
        else
            return new EntityActionInfo(statsTooAttack.user.Id, false);
    }

    public bool ValidateAttack(StatsManager userStats)
    {
        if (userStats.ManaManager.CurrentAmount - ManaReduction <= 0)
        {
            return false;
        }
        else return true;
    }
}
