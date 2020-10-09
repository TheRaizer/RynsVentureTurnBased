public class AttackAction : EntityAction
{
    /* The Method Parmeters:
     * statsTooActOn = StatsManager of the entity too attack.
     * scale = the damage scale of the entity using this attack.
     * 
     * Explanation:
     * These methods handle Critical and nonCritical attacks that reduce
     * the StatsTooActOn's healthmanager's current health.
     */

    public override ActionTypes ActionType { get; protected set; } = ActionTypes.Attack;

    protected override EntityActionInfo OnCrit(StatsManager statsTooActOn, float scale)
    {
        int damage = MathExtension.RoundToNearestInteger(amount * scale * critMultiplier);
        statsTooActOn.HealthManager.ReduceAmount(damage);

        return new EntityActionInfo(statsTooActOn.user.Id, damage, false);
    }

    protected override EntityActionInfo OnNonCrit(StatsManager statsTooActOn, float scale)
    {
        int damage = MathExtension.RoundToNearestInteger(amount * scale);
        statsTooActOn.HealthManager.ReduceAmount(damage);

        return new EntityActionInfo(statsTooActOn.user.Id, damage, false);
    }
}
