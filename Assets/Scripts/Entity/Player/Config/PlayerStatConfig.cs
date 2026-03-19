using UnityEngine;

[CreateAssetMenu(menuName = "StatConfig", fileName = "PlayerStatConfig")]
public class PlayerStatConfig : ScriptableObject
{
    public MovementConfig movementConfig;
    [Space(5)]
    public JumpConfig jumpConfig;

    [Header("Player stat")]
    [Min(1)] public int playerLife;
    [Min(0)] public float respawnDelay;
}