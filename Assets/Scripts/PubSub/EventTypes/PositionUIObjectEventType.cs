using UnityEngine;

public class PositionUIObjectEventType
{
    public readonly Directions directions;
    public readonly RectTransform UIRect;

    public PositionUIObjectEventType(Directions _directions, RectTransform _UIRect)
    {
        directions = _directions;
        UIRect = _UIRect;
    }
}
