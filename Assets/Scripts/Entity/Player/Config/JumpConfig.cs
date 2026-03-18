using UnityEngine;
[System.Serializable]
public struct JumpConfig : Config
{
    [Header("Jump Force")]
    [Min(0)] public float minimalJumpForce;
    [Min(1)] public float maximalJumpForce;
    public float holdForcePerSecond;

    [Header("Ground Detection")]
    public float checkIsGroundedRadius;
    public LayerMask jumpableLayer;

    [Header("Feel")]
    public float coyoteTime;
    public float jumpBufferTime;
    public float fallGravityMultiplier;
    public float cutJumpGravityMultiplier;
}