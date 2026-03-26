// JumpConfig.cs
using UnityEngine;

[System.Serializable]
public struct JumpConfig
{
    [Header("Jump Arc")]
    [Tooltip("Hauteur maximale atteinte au sommet du saut (en unités Unity)")]
    public float jumpApexHeight;
    [Tooltip("Temps en secondes pour atteindre le sommet du saut")]
    public float jumpApexTime;

    [Header("Jump Timing")]
    [Tooltip("Fenêtre de temps pendant laquelle le joueur peut sauter après avoir quitté le sol")]
    public float coyoteTime;
    [Tooltip("Fenêtre de temps pendant laquelle un input saut est mémorisé avant d'être exécuté")]
    public float jumpBufferTime;

    [Header("Gravity Modifiers")]
    [Tooltip("Multiplicateur de gravité appliqué en descente (> 1 = chute plus rapide)")]
    public float fallGravityMultiplier;
    [Tooltip("Multiplicateur de gravité appliqué quand le joueur relâche le saut en montée")]
    public float cutJumpGravityMultiplier;

    [Header("Feature Toggles")]
    [Tooltip("Active la fenêtre de coyote time")]
    public bool hasCoyoteTime;
    [Tooltip("Active les modificateurs de gravité en montée et en descente")]
    public bool hasJumpGravityModifiers;
}