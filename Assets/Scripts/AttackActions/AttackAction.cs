public class AttackAction : EntityAction
{
    public override ActionTypes ActionType { get; protected set; } = ActionTypes.Attack;

    protected override void OnCrit(StatsManager statsTooActOn, float scale)
    {
        statsTooActOn.HealthManager.ReduceAmount(MathExtension.RoundToNearestInteger(amount * scale * critMultiplier));
    }

    protected override void OnNonCrit(StatsManager statsTooActOn, float scale)
    {
        statsTooActOn.HealthManager.ReduceAmount(MathExtension.RoundToNearestInteger(amount * scale));
    }
}
