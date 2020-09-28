public class SupportAction : EntityAction
{
    public override ActionTypes ActionType { get; protected set; } = ActionTypes.Support;

    protected override void OnCrit(StatsManager statsTooActOn, float scale)
    {
        statsTooActOn.HealthManager.RegenAmount(MathExtension.RoundToNearestInteger(Amount * scale * CritMultiplier));
    }

    protected override void OnNonCrit(StatsManager statsTooActOn, float scale)
    {
        statsTooActOn.HealthManager.RegenAmount(MathExtension.RoundToNearestInteger(Amount * scale));
    }
}
