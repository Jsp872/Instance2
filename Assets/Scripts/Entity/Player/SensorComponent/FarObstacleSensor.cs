using UnityEngine;

public class FarObstacleSensor : SensorComponent
{
    public bool ObstacleDetected { get; private set; } = false;

    public override void InitializedSensorComponent(PlayerController controller)
    {
        base.InitializedSensorComponent(controller);

        controller.farObstacleSensor = this;
    }

    public override void OnResetSensor()
    {
        base.OnResetSensor();
        ObstacleDetected = false;
    }

    protected override void TriggerEntry(Collider2D collision)
    {
        ObstacleDetected = true;
        print("Obstacle is on detection range : " + collision?.name);
    }

    protected override void TriggerExit(Collider2D collision)
    {
        ObstacleDetected = false;
        print("Obstacle quit detection range : " + collision?.name);
    }
}