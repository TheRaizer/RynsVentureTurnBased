public class AttackInfo
{
    public AttackInfo(string _targetId, bool _hitTarget)
    {
        targetId = _targetId;
        hitTarget = _hitTarget;
    }
    public readonly string targetId;
    public readonly bool hitTarget;
}
