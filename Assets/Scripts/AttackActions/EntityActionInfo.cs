public class EntityActionInfo
{
    public EntityActionInfo(string _targetId, bool _hitTarget, int _amount, bool _support)
    {
        targetId = _targetId;
        hitTarget = _hitTarget;
        amount = _amount;
        support = _support;
    }
    public bool InflictedStatusEffect { get; set; }
    public bool CriticalHit { get; set; }
    public readonly string targetId;
    public readonly bool hitTarget;
    public readonly int amount;
    public readonly bool support;
}
