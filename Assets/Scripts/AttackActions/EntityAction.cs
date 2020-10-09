using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAction : MonoBehaviour
{
    /*
     * This is a abstract class that handles all types of entity actions
     * listed in the ActionTypes Enum below.
     * 
     * There are classes that extend this one which will be placed on the Unity
     * GameObject whose StatsManager corrospond to the entity that has these attacks.
     * 
     * The serialized properties and fields below will be filled inside Unity.
     */

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

    protected StatsManager userStats;

    public virtual void Awake()
    {
        userStats = GetComponent<StatsManager>();
    }

    protected bool RollForStatusEffect(StatsManager statsToApplyToo, bool instantApply, BattleTextBoxHandler textBoxHandler)
    {
        /*
         * statsToApplyToo = StatsManager to add the status effect too.
         * instantApply = Whether this should apply instantly without rolling for a chance.
         * textBoxHandler = class that controls the text that will be placed into the textBox.
         * 
         * This methods job is to roll a chance, and if it is less than or equal too this actions statusEffectChance
         * or instantApply is true than it will run StatusEffectApplication() method and return true. Else return false.
         */

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
        /*
         * statsTooApplyToo = StatsManager to add the effect too.
         * textBoxHandler = class that controls the text that will be placed into the textBox.
         * 
         * This methods job is to add text as infliction to the textBoxHandler as well as adding 
         * this actions status effect to the statsToApplyToo's StatusEffectManager's 
         * StatusEffectsDic. It will do this according to the type of status effect being applied.
         */

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

    public List<EntityActionInfo> UseAOEAction(List<StatsManager> statsToPerformActionOn, float damageScale, BattleTextBoxHandler textBoxHandler)
    {
        List<EntityActionInfo> actionInfos = new List<EntityActionInfo>();
        for (int i = 0; i < statsToPerformActionOn.Count; i++)
        {
            actionInfos.Add(UseAction(statsToPerformActionOn[i], damageScale, textBoxHandler));
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
                    hasInflicted = RollForStatusEffect(statsTooActOn, true, textBoxHandler);
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
                    hasInflicted = RollForStatusEffect(statsTooActOn, false, textBoxHandler);
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
