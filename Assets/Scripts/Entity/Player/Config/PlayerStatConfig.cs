using UnityEngine;

[CreateAssetMenu(menuName = "StatConfig", fileName = "PlayerStatConfig")]
public class PlayerStatConfig : ScriptableObject
{
    public MovementConfig movementConfig;
    public JumpConfig jumpConfig;
    public InteractionConfig interactionConfig;
    public SendNoteConfig sendNoteConfig;

    public void OnValidate()
    {
        movementConfig.moveSpeedDefaultValue = 
            Mathf.Clamp(movementConfig.moveSpeedDefaultValue, movementConfig.minMoveSpeed, movementConfig.maxMoveSpeed);

        jumpConfig.initialJumpForce =
            Mathf.Clamp(jumpConfig.initialJumpForce, jumpConfig.minimalJumpForce, jumpConfig.maximalJumpForce);
    }

}