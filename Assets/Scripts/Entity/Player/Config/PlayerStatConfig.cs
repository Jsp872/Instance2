using UnityEngine;

[CreateAssetMenu(menuName = "StatConfig", fileName = "PlayerStatConfig")]
public class PlayerStatConfig : EntityStatConfig
{
    [Space(10)]
    [Header("jumpValue")]
    public float initialJumpForce;
    [Min(0)] public float minimalJumpForce;
    [Min(1)] public float maximalJumpForce;
    [Min(0.0f)] public float jumpAcceleration;
    [Min(0.0f)] public float jumpdeceleration;
    public LayerMask jumpableLayer;
    public bool canWallJump = false;

    [Header("Interaction")]
    public LayerMask interactibleLayer;
    public float interactionRange;

    public override void OnValidate()
    {
        base.OnValidate();
        initialJumpForce = Mathf.Clamp(initialJumpForce, minimalJumpForce, maximalJumpForce);
    }
}