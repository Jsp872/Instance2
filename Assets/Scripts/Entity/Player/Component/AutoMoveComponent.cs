using UnityEngine;

public class AutoMoveComponent : PlayerComponent
{
    private float currentSpeed;
    private bool isDecelerating;
    private float obstacleDistance;

    public override void Initialize(PlayerStatConfig config, Rigidbody2D rb)
    {
        base.Initialize(config, rb);
        currentSpeed = config.moveSpeedDefaultValue;
    }

    public override void OnUpdated(ref Vector3 velocity, float fixedDeltaTime)
    {
        UpdateSpeed(fixedDeltaTime);
        velocity = playerConfig.movementDirection.normalized * currentSpeed;
    }

    private void UpdateSpeed(float fixedDeltaTime)
    {
        isDecelerating = IsObstacleAhead();

        if (isDecelerating)
        {
            float proximityRatio = Mathf.Clamp01(obstacleDistance / playerConfig.decelerationDistance);

            float decelerationMultiplier = Mathf.Lerp(playerConfig.maxMoveSpeed, playerConfig.decelerationValue, proximityRatio);

            currentSpeed -= decelerationMultiplier * fixedDeltaTime;
            currentSpeed = Mathf.Max(currentSpeed, playerConfig.minMoveSpeed);
        }
        else
        {
            currentSpeed += playerConfig.accelerationValue * fixedDeltaTime;
            currentSpeed = Mathf.Min(currentSpeed, playerConfig.maxMoveSpeed);
        }
    }

    private bool IsObstacleAhead()
    {
        Vector3 dir = playerConfig.movementDirection.normalized;
        Debug.DrawLine(transform.position, transform.position + playerConfig.decelerationDistance * dir, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            playerConfig.decelerationDistance,
            playerConfig.groundLayer
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