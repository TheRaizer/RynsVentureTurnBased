public class SupportAction : EntityAction
{
    public override ActionTypes ActionType { get; protected set; } = ActionTypes.Support;

    protected override EntityActionInfo OnCrit(StatsManager statsTooActOn, float scale)
    {
        int regenAmount = MathExtension.RoundToNearestInteger(amount * scale * critMultiplier);
        statsTooActOn.HealthManager.RegenAmount(regenAmount);
        return new EntityActionInfo(statsTooActOn.user.Id, true, regenAmount, true);
    }

    protected override EntityActionInfo OnNonCrit(StatsManager statsTooActOn, float scale)
    {
        int regenAmount = MathExtension.RoundToNearestInteger(amount * scale);
        statsTooActOn.HealthManager.RegenAmount(regenAmount);

        return new EntityActionInfo(statsTooActOn.user.Id, true, regenAmount, true);
    }
}
