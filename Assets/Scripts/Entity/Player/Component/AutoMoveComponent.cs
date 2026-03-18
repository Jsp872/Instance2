using UnityEngine;

public class AutoMoveComponent : PlayerComponent
{
    [SerializeField, Tooltip("Config Cache - changes don't save to SO!")]
    private MovementConfig config;

    private float currentSpeed;
    private float speedVelocity;

    private ObstacleInfo _nearest;

    private struct ObstacleInfo
    {
        public bool detected;
        public float distance;  
        public bool isPuzzle;
        public bool stopPlayer;
        public float detectionDistance;
        public float distanceMinSpeed;
    }

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        config = controller.GetConfig().movementConfig;
        currentSpeed = config.moveSpeedDefaultValue;
    }

    public override void UpdateComponent(ref Vector3 velocity, float dt)
    {
        _nearest = ScanObstacles();

        float targetSpeed = ComputeTargetSpeed();
        float smoothTime = targetSpeed < currentSpeed
            ? config.decelerationSmoothTime
            : config.accelerationSmoothTime;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothTime);
        velocity = config.movementDirection.normalized * currentSpeed;
    }

    private float ComputeTargetSpeed()
    {
        if (!_nearest.detected)
            return config.maxMoveSpeed;

        float distance = _nearest.distance;

        if (distance <= _nearest.distanceMinSpeed)
        {
            return _nearest.stopPlayer ? 0f : config.minMoveSpeed;
        }

        float t = Mathf.InverseLerp(_nearest.detectionDistance, _nearest.distanceMinSpeed, distance);
        return Mathf.Lerp(config.maxMoveSpeed, config.minMoveSpeed, t);
    }

    private ObstacleInfo ScanObstacles()
    {
        Vector3 dir = config.movementDirection.normalized;
        float maxDist = Mathf.Max(
            config.staticObstacleDetectionDistance,
            config.puzzleObstacleDetectionDistance
        );

        Debug.DrawLine(transform.position, transform.position + dir * maxDist, Color.green);

        ObstacleInfo result = default;
        result.distance = float.MaxValue;

        TryScan(dir, config.staticObstacleLayer, config.staticObstacleDetectionDistance,
                config.staticObstacleDistanceMinSpeed, config.stopPlayerOnStaticObstacle,
                isPuzzle: false, ref result);

        TryScan(dir, config.puzzleObstacleLayer, config.puzzleObstacleDetectionDistance,
                config.puzzleDistanceMinSpeed, config.stopPlayerOnPuzzleObstacle,
                isPuzzle: true, ref result);

        return result;
    }

    private void TryScan(Vector3 dir, LayerMask layer, float detectionDist, float distMinSpeed, 
        bool stopPlayer, bool isPuzzle, ref ObstacleInfo current)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, detectionDist, layer);
        if (!hit.collider) return;

        if (hit.distance < current.distance)
        {
            current.detected = true;
            current.distance = hit.distance;
            current.isPuzzle = isPuzzle;
            current.stopPlayer = stopPlayer;
            current.detectionDistance = detectionDist;
            current.distanceMinSpeed = distMinSpeed;
        }
    }
}