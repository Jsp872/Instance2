using UnityEngine;
[System.Serializable]
public struct MovementConfig : Config
{
    [Header("Movement")]
    public Vector3 movementDirection;
    public LayerMask groundLayer;
    public float moveSpeedDefaultValue;
    [Min(0)] public float minMoveSpeed;
    public float maxMoveSpeed;
    public float accelerationSmoothTime;
    public float decelerationSmoothTime;

    [Header("Static Obstacle")]
    public LayerMask staticObstacleLayer;
    public float staticObstacleDetectionDistance;
    public float staticObstacleDistanceMinSpeed;
    public bool stopPlayerOnStaticObstacle;

    [Header("Puzzle Obstacle")]
    public LayerMask puzzleObstacleLayer;
    public float puzzleObstacleDetectionDistance;
    public float puzzleDistanceMinSpeed;
    public bool stopPlayerOnPuzzleObstacle;
}