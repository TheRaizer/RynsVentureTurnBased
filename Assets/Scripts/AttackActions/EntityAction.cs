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
    [field: SerializeField] public int ManaReduction { get; private set; }
    [field: SerializeField] public string TriggerName { get; private set; }
    [field: SerializeField] public AnimationClip AnimToPlay { get; private set; }

    [SerializeField] protected string actionText = "";
    [SerializeField] protected int amount = 0;
    [SerializeField] protected float accuracy = 0;
    [SerializeField] protected float critChance = 0;
    [SerializeField] protected float critMultiplier = 0;
    [SerializeField] protected GameObject statusEffectPrefab = null;
    [SerializeField] private float statusEffectChance = 0;

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

            if (chance <= statusEffectChance)
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
        StatusEffect s = statusEffectPrefab.GetComponent<StatusEffect>();
        textBoxHandler.AddTextAsStatusInfliction(userStats.user.Id, statsToApplyToo.user.Id, s.Name);
        Debug.Log("Apply " + s.Name + " too " + statsToApplyToo.user.Id);

        if(s.EffectType == EffectType.ReplaceTurn)
        {
            statsToApplyToo.StatusEffectsManager.AddToReplacementTurn(s.ShallowCopy());
        }
        else
        {
            statsToApplyToo.StatusEffectsManager.AddToStatusEffectsDic(s.EffectType, s.ShallowCopy());
        }
    }

    public List<EntityActionInfo> DetermineAction(List<StatsManager> statsTooPerformActionOn, float damageScale, BattleTextBoxHandler textBoxHandler, int indexToActOn = 0)
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

        textBoxHandler.AddTextAsAttack(userStats.user.Id, actionText, statsTooActOn.user.Id);

        if (chance < accuracy)
        {
            int critChance = UnityEngine.Random.Range(0, 100);

            if (critChance < this.critChance)
            {
                EntityActionInfo actionInfo = OnCrit(statsTooActOn, scale);
                textBoxHandler.AddTextAsCriticalHit();
                if (statusEffectPrefab != null)
                {
                    hasInflicted = ApplyStatusEffect(statsTooActOn, true, textBoxHandler);
                }
                Debug.Log("Critical hit");
                actionInfo.CriticalHit = true;
                actionInfo.InflictedStatusEffect = hasInflicted;
                return actionInfo;
            }
            else
            {
                EntityActionInfo actionInfo = OnNonCrit(statsTooActOn, scale);
                if (statusEffectPrefab != null)
                {
                    hasInflicted = ApplyStatusEffect(statsTooActOn, false, textBoxHandler);
                }
                actionInfo.InflictedStatusEffect = hasInflicted;
                return actionInfo;
            }
        }
        else
        {
            textBoxHandler.AddTextOnMiss(userStats.user.Id, statsTooActOn.user.Id);
            EntityActionInfo actionInfo = new EntityActionInfo(statsTooActOn.user.Id, 0, false, false)
            {
                InflictedStatusEffect = false
            };
            return actionInfo;
        }
    }

    protected abstract EntityActionInfo OnCrit(StatsManager statsTooActOn, float scale);
    protected abstract EntityActionInfo OnNonCrit(StatsManager statsTooActOn, float scale);
}
