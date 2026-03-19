using UnityEngine;

public class AutoMoveComponent : PlayerComponent
{
    //[Header("Detection Zone ")]
    //[SerializeField] private Transform obstacleDetector;
    //[SerializeField] private Transform playerDeathDetection;


    [SerializeField, Tooltip("Config Cache - changes don't save to SO!")]
    private MovementConfig config;

    [field: Header("_____DEBUG_____")]
    [field: SerializeField]
    public float currentSpeed { get; private set; }

    [SerializeField] private bool hasReachedMaxSpeed = false;
    // [SerializeField] private bool wasInObstacleRange = false;
    private bool callEventOnce;

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        config = controller.GetConfig().movementConfig;
        currentSpeed = config.defaultMoveSpeed;
    }

    public override void UpdateComponent(ref Vector3 velocity, float dt)
    {
        bool isOnObstacleRange = IsInObstacleRange();
        bool hitObstacle = IsHitObstacle();

        if (hitObstacle)
        {
            OnHitObstacle();
            return;
        }

        if (isOnObstacleRange)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                config.playerObstacleRangeSpeed,
                config.playerDeceleration * dt
            );
            // wasInObstacleRange = true;
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
                OnMaxSpeedReach();
                callEventOnce = true;
            }

            // wasInObstacleRange = false;
        }

        velocity = config.defaultMovementDir.normalized * currentSpeed;
    }

    private void OnMaxSpeedReach()
    {
        // Call start MaxSpeed feedback, animation or sfx
        Debug.Log("[AutoMove] Max speed reached!");
        EventBus.Publish(new MaxSpeedReachCallback());
    }
    private void OnLooseMaxSpeed()
    {
        //cancel Max Speed rech CTX
        Debug.Log("[AutoMove] Loose Max speed");
        EventBus.Publish(new LooseMaxSpeedCallback());
    }
    private void OnHitObstacle()
    {
        // Call Death Event
        Debug.Log("[AutoMove] Hit obstacle � death!");
        EventBus.Publish(new OnHitObstacleCallback());
    }

    public bool IsHitObstacle()
    {
        RaycastHit2D hit = MultyRaycastUtils.MultiRaycast(
            origin: transform,
            direction: config.defaultMovementDir.normalized,
            distance: config.playerDeathRange,
            count: config.obstacleRaycastCount,
            spreadAxis: Vector2.up,
            spread: config.raycastOffset,
            layerMask: playerStatConfig.collisionLayers
        );
        return hit;
    }

    public bool IsInObstacleRange()
    {
        RaycastHit2D hit = MultyRaycastUtils.MultiRaycast(
            origin: transform,
            direction: config.defaultMovementDir.normalized,
            distance: config.obstacleDetectionDistance,
            count: config.obstacleRaycastCount,
            spreadAxis: Vector2.up,
            spread: config.raycastOffset,
            layerMask: playerStatConfig.collisionLayers
        );
        return hit;
    }
}