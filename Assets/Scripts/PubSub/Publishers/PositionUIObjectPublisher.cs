
public class PositionUIObjectPublisher
{
    public void Publish(PositionUIObjectEventType positionUIObjectEventType)
    {
        EventAggregator.SingleInstance.Publish(positionUIObjectEventType);
    }
}
