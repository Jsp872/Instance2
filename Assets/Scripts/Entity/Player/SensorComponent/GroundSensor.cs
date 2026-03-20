using UnityEngine;

public class GroundSensor : SensorComponent
{
    public bool IsGrounded { get; private set; } = false;
    private LayerMask _spikeLayer;

    public override void InitializedSensorComponent(PlayerController controller)
    {
        base.InitializedSensorComponent(controller);
        _spikeLayer = controller.GetConfig().spikeLayer;

        controller.groundSensor = this;
    }
    public override void OnResetSensor()
    {
        base.OnResetSensor();
        IsGrounded = false;
    }


    protected override void CollisionEntry(Collision2D collision)
    {
        IsGrounded = true;

        if (IsInLayer(collision.gameObject.layer, _spikeLayer))
            EventBus.Publish(new OnHitObstacleCallback());
    }

    protected override void CollisionExit(Collision2D collision)
    {
        IsGrounded = false;
    }
}