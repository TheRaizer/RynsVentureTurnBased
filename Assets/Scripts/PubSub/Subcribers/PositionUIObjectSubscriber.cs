using UnityEngine;

public class PositionUIObjectSubscriber : MonoBehaviour
{
    private void Start()
    {
        EventAggregator.SingleInstance.Subscribe<PositionUIObjectEventType>(PositionUIObject);
    }

    private void PositionUIObject(PositionUIObjectEventType UIObjectMessage)
    {
        Directions directions = UIObjectMessage.directions;
        RectTransform rect = UIObjectMessage.UIRect;

        RectTransformExtensions.SetBottom(rect, directions.bottom);
        RectTransformExtensions.SetTop(rect, directions.top);
        RectTransformExtensions.SetLeft(rect, directions.left);
        RectTransformExtensions.SetRight(rect, directions.right);
    }
}