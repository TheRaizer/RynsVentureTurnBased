public class AttackAction : EntityAction
{
    public override ActionTypes ActionType { get; protected set; } = ActionTypes.Attack;

    protected override EntityActionInfo OnCrit(StatsManager statsTooActOn, float scale)
    {
        int damage = MathExtension.RoundToNearestInteger(amount * scale * critMultiplier);
        statsTooActOn.HealthManager.ReduceAmount(damage);

        return new EntityActionInfo(statsTooActOn.user.Id, true, damage, false);
    }

    protected override EntityActionInfo OnNonCrit(StatsManager statsTooActOn, float scale)
    {
        int damage = MathExtension.RoundToNearestInteger(amount * scale);
        statsTooActOn.HealthManager.ReduceAmount(damage);

        return new EntityActionInfo(statsTooActOn.user.Id, true, damage, false);
    }
}
