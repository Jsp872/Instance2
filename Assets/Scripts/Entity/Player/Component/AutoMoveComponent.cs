using UnityEngine;

public class AutoMoveComponent : PlayerComponent
{
    [SerializeField, Tooltip("Config Cache - changes don't save to SO!")]
    private MovementConfig config;

    [field: Header("_____DEBUG_____")]
    [field: SerializeField] public float currentSpeed { get; private set; }
    [SerializeField] private bool hasReachedMaxSpeed = false;

    private bool callEventOnce;

    private NearObstacleSensor _nearSensor;
    private FarObstacleSensor _farSensor;

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        config = controller.GetConfig().movementConfig;
        currentSpeed = config.defaultMoveSpeed;

        _nearSensor = controller.nearObstacleSensor;
        _farSensor = controller.farObstacleSensor;
    }

    public override void UpdateComponent(ref Vector3 velocity, float dt)
    {
        //bool hitObstacle = _nearSensor.ObstacleDetected;
        bool isOnObstacleRange = _farSensor.ObstacleDetected;

        //if (hitObstacle)
        //{
        //    OnHitObstacle();
        //    return;
        //}

        if (isOnObstacleRange)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                config.playerObstacleRangeSpeed,
                config.playerDeceleration * dt
            );

            hasReachedMaxSpeed = false;

            if (callEventOnce)
            {
                OnLooseMaxSpeed();
                callEventOnce = false;
            }
        }
        else
        {
            currentSpeed += config.accelerationSmoothTime * dt;
            currentSpeed = Mathf.Clamp(currentSpeed, config.defaultMoveSpeed, config.maxMoveSpeed);

            if (!hasReachedMaxSpeed && Mathf.Approximately(currentSpeed, config.maxMoveSpeed))
            {
                hasReachedMaxSpeed = true;
                callEventOnce = true;
                OnMaxSpeedReach();
            }
        }

        velocity = config.defaultMovementDir.normalized * currentSpeed;
    }

    private void OnMaxSpeedReach()
    {
        Debug.Log("[AutoMove] Max speed reached!");
        EventBus.Publish(new MaxSpeedReachCallback());
    }

    private void OnLooseMaxSpeed()
    {
        EventBus.Publish(new LooseMaxSpeedCallback());
    }
    
    //private void OnHitObstacle()
    //{
    //    if (enabled)
    //    {
    //        // Call Death Event
    //        Debug.Log("[AutoMove] Hit obstacle!");
    //        EventBus.Publish(new OnHitObstacleCallback());
    //        enabled = false;
    //    }
    //}

    public override void OnPlayerRespawn()
    {
        base.OnPlayerRespawn();
        enabled = true;
    }
}