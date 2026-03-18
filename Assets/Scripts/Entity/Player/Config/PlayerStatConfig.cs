using UnityEngine;

[CreateAssetMenu(menuName = "StatConfig", fileName = "PlayerStatConfig")]
public class PlayerStatConfig : ScriptableObject
{
    public MovementConfig movementConfig;
    [Space(5)]
    public JumpConfig jumpConfig;
    [Space(5)]
    public InteractionConfig interactionConfig;
    [Space(5)]
    public SendNoteConfig sendNoteConfig;



    public void OnValidate()
    {
        movementConfig.moveSpeedDefaultValue =
            Mathf.Clamp(movementConfig.moveSpeedDefaultValue, movementConfig.minMoveSpeed, movementConfig.maxMoveSpeed);

        jumpConfig.minimalJumpForce =
            Mathf.Clamp(jumpConfig.minimalJumpForce, jumpConfig.minimalJumpForce, jumpConfig.maximalJumpForce);


        sendNoteConfig.isHoldDelay =
            Mathf.Clamp(sendNoteConfig.isHoldDelay, 0, sendNoteConfig.combosDelay);
    }
}