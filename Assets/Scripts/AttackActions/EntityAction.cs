using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAction : MonoBehaviour
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public bool IsAOE { get; private set; }
    [field: SerializeField] public bool IsSupport { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
    [field: SerializeField] public int ManaReduction { get; private set; }
    [field: SerializeField] public float Accuracy { get; private set; }
    [field: SerializeField] public GameObject StatusEffectPrefab { get; private set; }
    [field: SerializeField] public float StatusEffectChance { get; private set; }
    [field: SerializeField] public float CritChance { get; private set; }
    [field: SerializeField] public float CritMultiplier { get; private set; }
    [field: SerializeField] public string AttackText { get; private set; }

    [SerializeField] private StatsManager userStats;

    public void Awake()
    {
        userStats = GetComponent<StatsManager>();
    }

    private bool ApplyStatusEffect(StatsManager statsToApplyToo, bool instantApply, BattleTextBoxHandler textBoxHandler)
    {
        if (!instantApply)
        {
            float chance = UnityEngine.Random.Range(0, 100);

            if (chance <= StatusEffectChance)
            {
                StatusEffectApplication(statsToApplyToo, textBoxHandler);
                return true;
            }

            return false;
        }
        else
        {
            StatusEffectApplication(statsToApplyToo, textBoxHandler);
            return true;
        }

    }

    private void StatusEffectApplication(StatsManager statsToApplyToo, BattleTextBoxHandler textBoxHandler)
    {
        StatusEffect s = StatusEffectPrefab.GetComponent<StatusEffect>();
        textBoxHandler.AddTextAsStatusInfliction(userStats.user.Id, statsToApplyToo.user.Id, s.Name);
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

    public List<EntityActionInfo> DetermineAction(List<StatsManager> statsTooAttack, float damageScale, int indexToAttack, BattleTextBoxHandler textBoxHandler)
    {
        List<EntityActionInfo> attackInfos = new List<EntityActionInfo>();
        if(!IsAOE)
        {
            attackInfos.Add(UseAction(statsTooAttack[indexToAttack], damageScale, textBoxHandler));
        }
        else
        {
            for(int i = 0; i < statsTooAttack.Count; i++)
            {
                attackInfos.Add(UseAction(statsTooAttack[i], damageScale, textBoxHandler));
            }
        }

        return attackInfos;
    }

    public EntityActionInfo UseAction(StatsManager statsTooAttack, float scale, BattleTextBoxHandler textBoxHandler)
    {
        int chance = UnityEngine.Random.Range(0, 100);
        bool hasInflicted = false;

        textBoxHandler.AddTextAsAttack(userStats.user.Id, AttackText, statsTooAttack.user.Id);

        if (chance < Accuracy)
        {
            int critChance = UnityEngine.Random.Range(0, 100);

            if (critChance < CritChance)
            {
                if (IsSupport)
                {
                    statsTooAttack.HealthManager.RegenAmount(MathExtension.RoundToNearestInteger(Amount * scale * CritMultiplier));
                }
                else
                {
                    statsTooAttack.HealthManager.ReduceAmount(MathExtension.RoundToNearestInteger(Amount * scale * CritMultiplier));
                }
                textBoxHandler.AddTextAsCriticalHit();

                if (StatusEffectPrefab != null)
                {
                    hasInflicted = ApplyStatusEffect(statsTooAttack, true, textBoxHandler);
                }
                Debug.Log("Critical hit");
                return new EntityActionInfo(statsTooAttack.user.Id, true, hasInflicted);
            }
            else
            {
                if (IsSupport)
                {
                    statsTooAttack.HealthManager.RegenAmount(MathExtension.RoundToNearestInteger(Amount * scale));
                }
                else
                {
                    statsTooAttack.HealthManager.ReduceAmount(MathExtension.RoundToNearestInteger(Amount * scale));
                }

                if (StatusEffectPrefab != null)
                {
                    hasInflicted = ApplyStatusEffect(statsTooAttack, false, textBoxHandler);
                }
                return new EntityActionInfo(statsTooAttack.user.Id, true, hasInflicted);
            }
        }
        else
        {
            textBoxHandler.AddTextOnMiss(userStats.user.Id, statsTooAttack.user.Id);
            return new EntityActionInfo(statsTooAttack.user.Id, false, false);
        }
    }

    public bool ValidateManaForAction(StatsManager userStats)
    {
        if (userStats.ManaManager.CurrentAmount - ManaReduction <= 0)
        {
            return false;
        }
        else return true;
    }
}
