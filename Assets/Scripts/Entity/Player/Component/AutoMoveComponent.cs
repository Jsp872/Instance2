using UnityEngine;

public class AutoMoveComponent : PlayerComponent
{
    [Header("DEBUG")]
    [SerializeField] private MovementConfig configCopy;

    private float currentSpeed;
    private bool isDecelerating;
    private float obstacleDistance;

    public override void Initialize(PlayerStatConfig config, Rigidbody2D rb)
    {
        base.Initialize(config, rb);
        configCopy = config.movementConfig;
        currentSpeed = configCopy.moveSpeedDefaultValue;
    }

    public override void OnUpdated(ref Vector3 velocity, float fixedDeltaTime)
    {
        UpdateSpeed(fixedDeltaTime);
        velocity = configCopy.movementDirection.normalized * currentSpeed;
    }

    private void UpdateSpeed(float fixedDeltaTime)
    {
        isDecelerating = IsObstacleAhead();

        if (isDecelerating)
        {
            float proximityRatio = Mathf.Clamp01(obstacleDistance / configCopy.decelerationDistance);

            float decelerationMultiplier = Mathf.Lerp(configCopy.maxMoveSpeed, configCopy.decelerationValue, proximityRatio);

            currentSpeed -= decelerationMultiplier * fixedDeltaTime;
            currentSpeed = Mathf.Max(currentSpeed, configCopy.minMoveSpeed);
        }
        else
        {
            currentSpeed += configCopy.accelerationValue * fixedDeltaTime;
            currentSpeed = Mathf.Min(currentSpeed, configCopy.maxMoveSpeed);
        }
    }

    private bool IsObstacleAhead()
    {
        Vector3 dir = configCopy.movementDirection.normalized;
        Debug.DrawLine(transform.position, transform.position + configCopy.decelerationDistance * dir, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            configCopy.decelerationDistance,
            configCopy.groundLayer
        );

        if (hit.transform != null)
        {
            obstacleDistance = Vector2.Distance(transform.position, hit.transform.position);
        }
        else
            obstacleDistance = -100.0f;

        return hit.collider != null;
    }
}