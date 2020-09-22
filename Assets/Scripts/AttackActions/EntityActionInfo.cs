public class EntityActionInfo
{
    public EntityActionInfo(string _targetId, bool _hitTarget, bool _inflictedStatusEffect)
    {
        targetId = _targetId;
        hitTarget = _hitTarget;
        inflictedStatusEffect = _inflictedStatusEffect;
    }
    public readonly string targetId;
    public readonly bool hitTarget;

    public readonly bool inflictedStatusEffect;
}
