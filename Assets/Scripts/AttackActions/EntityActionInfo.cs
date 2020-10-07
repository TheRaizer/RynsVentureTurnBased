public class EntityActionInfo
{
    public EntityActionInfo(string _targetId, int _amount, bool _support, bool _hitTarget = true, bool _skip = false)
    {
        targetId = _targetId;
        hitTarget = _hitTarget;
        amount = _amount;
        support = _support;
        skip = _skip;
    }
    public bool InflictedStatusEffect { get; set; }
    public bool CriticalHit { get; set; }
    public readonly string targetId;
    public readonly bool hitTarget;
    public readonly int amount;
    public readonly bool support;
    public readonly bool skip;
}
