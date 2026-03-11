using UnityEngine;

[CreateAssetMenu(menuName = "StatConfig", fileName = "PlayerStatConfig")]
public class PlayerStatConfig : EntityStatConfig
{
    [Header("jumpValue")]
    public float initialJumpForce;
    [Range(0, 1)] public float minimalJumpForce;
    [Range(1, 5)] public float maximalJumpForce;
    [Min(0.0f)] public float jumpAcceleration;
    [Min(0.0f)] public float jumpdeceleration;

    public LayerMask jumpableLayer;
    public bool canWallJump = false;

    public override void OnValidate()
    {
        base.OnValidate();
        initialJumpForce = Mathf.Clamp(initialJumpForce, minimalJumpForce, maximalJumpForce);
    }
}