// MovementConfig.cs
using UnityEngine;

[System.Serializable]
public struct MovementConfig
{
    [Header("Movement")]
    public Vector3 defaultMovementDir;
    public float defaultMoveSpeed;
    public float maxMoveSpeed;
    public float accelerationSmoothTime;

    [Header("Obstacle")]
    public float playerObstacleRangeSpeed;
    public float playerDeceleration;
}