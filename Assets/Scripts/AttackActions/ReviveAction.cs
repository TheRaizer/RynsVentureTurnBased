using UnityEngine;

public class ReviveAction : EntityAction
{
    public override ActionTypes ActionType { get; protected set; } = ActionTypes.Revive;
    [SerializeField] private float percentHealthAfterRevive;

    protected override void OnCrit(StatsManager statsTooActOn, float scale)
    {
        statsTooActOn.HealthManager.Dead = false;
        statsTooActOn.HealthManager.RegenAmount(MathExtension.RoundToNearestInteger(statsTooActOn.HealthManager.MaxAmount * percentHealthAfterRevive * CritMultiplier));
    }

    protected override void OnNonCrit(StatsManager statsTooActOn, float scale)
    {
        statsTooActOn.HealthManager.Dead = false;
        statsTooActOn.HealthManager.RegenAmount(MathExtension.RoundToNearestInteger(statsTooActOn.HealthManager.MaxAmount * percentHealthAfterRevive));
    }
}
