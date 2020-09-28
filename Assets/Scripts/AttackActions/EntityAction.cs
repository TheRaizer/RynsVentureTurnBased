using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAction : MonoBehaviour
{
    public enum ActionTypes
    {
        Support,
        Attack,
        Revive
    }
    public abstract ActionTypes ActionType { get; protected set; }
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public bool IsAOE { get; private set; }

    [SerializeField] protected string ActionText;
    [SerializeField] protected int ManaReduction;
    [SerializeField] protected int Amount;
    [SerializeField] protected float Accuracy;
    [SerializeField] protected float CritChance;
    [SerializeField] protected float CritMultiplier;
    [SerializeField] protected GameObject StatusEffectPrefab;
    [SerializeField] private float StatusEffectChance;

    [SerializeField] protected StatsManager userStats;

    public virtual void Awake()
    {
        userStats = GetComponent<StatsManager>();
    }

    protected bool ApplyStatusEffect(StatsManager statsToApplyToo, bool instantApply, BattleTextBoxHandler textBoxHandler)
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

    protected void StatusEffectApplication(StatsManager statsToApplyToo, BattleTextBoxHandler textBoxHandler)
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

    public List<EntityActionInfo> DetermineAction(List<StatsManager> statsTooPerformActionOn, float damageScale, int indexToActOn, BattleTextBoxHandler textBoxHandler)
    {
        List<EntityActionInfo> actionInfos = new List<EntityActionInfo>();
        if(!IsAOE)
        {
            actionInfos.Add(UseAction(statsTooPerformActionOn[indexToActOn], damageScale, textBoxHandler));
        }
        else
        {
            for(int i = 0; i < statsTooPerformActionOn.Count; i++)
            {
                actionInfos.Add(UseAction(statsTooPerformActionOn[i], damageScale, textBoxHandler));
            }
        }

        return actionInfos;
    }

    public EntityActionInfo UseAction(StatsManager statsTooActOn, float scale, BattleTextBoxHandler textBoxHandler)
    {
        userStats.ManaManager.ReduceAmount(ManaReduction);

        int chance = UnityEngine.Random.Range(0, 100);
        bool hasInflicted = false;

        textBoxHandler.AddTextAsAttack(userStats.user.Id, ActionText, statsTooActOn.user.Id);

        if (chance < Accuracy)
        {
            int critChance = UnityEngine.Random.Range(0, 100);

            if (critChance < CritChance)
            {
                OnCrit(statsTooActOn, scale);
                textBoxHandler.AddTextAsCriticalHit();
                if (StatusEffectPrefab != null)
                {
                    hasInflicted = ApplyStatusEffect(statsTooActOn, true, textBoxHandler);
                }
                Debug.Log("Critical hit");
                return new EntityActionInfo(statsTooActOn.user.Id, true, hasInflicted);
            }
            else
            {
                OnNonCrit(statsTooActOn, scale);
                if (StatusEffectPrefab != null)
                {
                    hasInflicted = ApplyStatusEffect(statsTooActOn, false, textBoxHandler);
                }
                return new EntityActionInfo(statsTooActOn.user.Id, true, hasInflicted);
            }
        }
        else
        {
            textBoxHandler.AddTextOnMiss(userStats.user.Id, statsTooActOn.user.Id);
            return new EntityActionInfo(statsTooActOn.user.Id, false, false);
        }
    }

    protected abstract void OnCrit(StatsManager statsTooActOn, float scale);
    protected abstract void OnNonCrit(StatsManager statsTooActOn, float scale);

    public virtual bool ValidateManaForAction(StatsManager userStats)
    {
        if (userStats.ManaManager.CurrentAmount - ManaReduction <= 0)
        {
            return false;
        }
        else return true;
    }
}
