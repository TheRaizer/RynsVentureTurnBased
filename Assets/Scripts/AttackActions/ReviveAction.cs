using UnityEngine;

public class ReviveAction : EntityAction
{
    public override ActionTypes ActionType { get; protected set; } = ActionTypes.Revive;
    [SerializeField] private float percentHealthAfterRevive = 0;

    protected override EntityActionInfo OnCrit(StatsManager statsTooActOn, float scale)
    {
        statsTooActOn.HealthManager.Dead = false;

        int regenAmount = MathExtension.RoundToNearestInteger(statsTooActOn.HealthManager.MaxAmount * percentHealthAfterRevive * critMultiplier);
        statsTooActOn.HealthManager.RegenAmount(regenAmount);

        return new EntityActionInfo(statsTooActOn.user.Id, regenAmount, true);
    }

    protected override EntityActionInfo OnNonCrit(StatsManager statsTooActOn, float scale)
    {
        statsTooActOn.HealthManager.Dead = false;
        int regenAmount = MathExtension.RoundToNearestInteger(statsTooActOn.HealthManager.MaxAmount * percentHealthAfterRevive);

        statsTooActOn.HealthManager.RegenAmount(regenAmount);

        return new EntityActionInfo(statsTooActOn.user.Id, regenAmount, true);
    }
}
