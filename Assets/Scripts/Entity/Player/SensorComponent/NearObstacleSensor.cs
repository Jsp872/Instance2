using UnityEngine;

public class NearObstacleSensor : SensorComponent
{
    public override void InitializedSensorComponent(PlayerController controller)
    {
        base.InitializedSensorComponent(controller);

        controller.nearObstacleSensor = this;
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        EventBus.Publish(new OnHitObstacleCallback());
    }
}