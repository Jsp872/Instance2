// MovementConfig.cs
using UnityEngine;

[System.Serializable]
public struct MovementConfig
{
    [Header("Movement")]

    [Tooltip("Default direction the player moves (normalized vector)")]
    public Vector3 defaultMovementDir;
    [Tooltip("Base movement speed of the player (units/second)")]
    public float defaultMoveSpeed;
    [Tooltip("Maximum speed the player can reach (units/second)")]
    public float maxMoveSpeed;
    [Tooltip("Smoothing time for acceleration transitions (lower = snappier)")]
    public float accelerationSmoothTime;

    [Space(1)]

    [Header("Obstacle")]

    [Tooltip("Speed multiplier applied when the player is near an obstacle")]
    public float playerObstacleRangeSpeed;
    [Tooltip("Deceleration rate when the player approaches an obstacle (units/second²)")]
    public float playerDeceleration;
}