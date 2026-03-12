using UnityEngine;

[CreateAssetMenu(menuName = "StatConfig", fileName = "PlayerStatConfig")]
public class PlayerStatConfig : ScriptableObject
{
    public MovementConfig movementConfig;
    public JumpConfig jumpConfig;
    public InteractionConfig interactionConfig;
}