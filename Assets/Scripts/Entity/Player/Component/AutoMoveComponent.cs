using UnityEngine;

public class AutoMoveComponent : PlayerComponent
{
    [SerializeField, Tooltip("Config Cache - changes don't save to SO!")]
    private MovementConfig config;

    [Header("_____DEBUG_____")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private bool hasReachedMaxSpeed = false;
    [SerializeField] private bool wasInObstacleRange = false;
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
            wasInObstacleRange = true;
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

            wasInObstacleRange = false;
        }

        velocity = config.defaultMovementDir.normalized * currentSpeed;
    }

    public struct MaxSpeedReachCallback { }
    private void OnMaxSpeedReach()
    {
        // Call start MaxSpeed feedback, animation or sfx
        Debug.Log("[AutoMove] Max speed reached!");
        EventBus.Publish(new MaxSpeedReachCallback());
    }
    public struct LooseMaxSpeedCallback { }
    private void OnLooseMaxSpeed()
    {
        //cancel Max Speed rech CTX
        Debug.Log("[AutoMove] Loose Max speed");
        EventBus.Publish(new LooseMaxSpeedCallback());
    }
    public struct HitObstacleCallback { }
    private void OnHitObstacle()
    {
        // Call Death Event
        Debug.Log("[AutoMove] Hit obstacle — death!");
        EventBus.Publish(new HitObstacleCallback());
    }

    public bool IsHitObstacle()
    {
#if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.position + config.defaultMovementDir.normalized * config.playerDeathRange, Color.blue);
#endif
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            config.defaultMovementDir.normalized,
            config.playerDeathRange,
            config.obstacleLayer
            );
        return hit.collider != null;
    }

    public bool IsInObstacleRange()
    {
#if UNITY_EDITOR
        Vector3 Yoffset = new Vector3(0, 1, 0);
        Debug.DrawLine(transform.position + Yoffset, transform.position + Yoffset + config.defaultMovementDir.normalized * config.obstacleDetectionDistance, Color.red);
#endif
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
             config.defaultMovementDir.normalized,
             config.obstacleDetectionDistance,
             config.obstacleLayer
             );

        return hit.collider != null;
    }
}