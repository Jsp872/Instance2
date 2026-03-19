using UnityEngine;

[CreateAssetMenu(menuName = "StatConfig", fileName = "PlayerStatConfig")]
public class PlayerStatConfig : ScriptableObject
{
    public MovementConfig movementConfig;
    [Space(5)]
    public JumpConfig jumpConfig;
    [Space(5)]
    public SendNoteConfig sendNoteConfig;
}