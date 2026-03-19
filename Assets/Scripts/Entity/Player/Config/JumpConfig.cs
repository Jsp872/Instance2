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
    [Tooltip("Fenêtre de temps (en secondes) pendant laquelle le joueur peut sauter après avoir quitté le sol")]
    public float coyoteTime;
    [Tooltip("Fenêtre de temps (en secondes) pendant laquelle un input saut est mémorisé avant d'être exécuté")]
    public float jumpBufferTime;

    [Header("Ground Detection")]
    [Tooltip("Longueur du raycast vers le bas pour détecter le sol")]
    public float checkIsGroundedRadius;
    public float raycastOffset;
    [Tooltip("Nombre de raycasts pour la détection du sol")]
    public int groundedRaycastCount;

    [Header("Gravity Modifiers")]
    [Tooltip("Multiplicateur de gravité appliqué en descente (> 1 = chute plus rapide)")]
    public float fallGravityMultiplier;
    [Tooltip("Multiplicateur de gravité appliqué quand le joueur relâche le saut en montée (coupe le saut)")]
    public float cutJumpGravityMultiplier;

    [Header("Feature Toggles")]
    [Tooltip("Active la fenêtre de coyote time — permet de sauter brièvement après avoir quitté un bord")]
    public bool hasCoyoteTime;
    [Tooltip("Active les modificateurs de gravité en montée et en descente")]
    public bool hasJumpGravityModifiers;
}