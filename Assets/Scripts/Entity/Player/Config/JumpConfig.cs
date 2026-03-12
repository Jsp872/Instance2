using UnityEngine;


[System.Serializable]
public struct JumpConfig : Config
{
    [Header("jumpValue")]
    public float initialJumpForce;
    [Min(0)] public float minimalJumpForce;
    [Min(1)] public float maximalJumpForce;
    [Min(0.0f)] public float jumpAcceleration;

    public float holdJumpStartTime;
    public float checkIsGroundedRadius;

    public LayerMask jumpableLayer;
    public bool canWallJump;
}