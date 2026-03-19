using UnityEngine;
[System.Serializable]
public struct MovementConfig
{
    [Header("Movement")]
    public Vector3 defaultMovementDir;
    public float defaultMoveSpeed;
    public float maxMoveSpeed;
    public float accelerationSmoothTime;

    [Header("Detection et Feedback liée aux obstacle")]
    public float obstacleDetectionDistance;
    public float playerObstacleRangeSpeed;
    public float playerDeceleration;
    [Tooltip("Distance minimale entre le joueur et un obstacle avant la mort (en unités Unity)")]
    public float playerDeathRange;

    [Header("RayCast Detection offset")]
    public float raycastOffset;
    [Tooltip("Nombre de raycasts pour la détection d'obstacle")]
    public int obstacleRaycastCount;

    //[Header("Obstacle / Wall Layers")]
    //public LayerMask obstacleLayer;
}