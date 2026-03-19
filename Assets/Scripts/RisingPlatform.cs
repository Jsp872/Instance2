using DG.Tweening;
using UnityEngine;

/// <summary>
/// Plateforme qui s'élève lors du déverrouillage.
/// </summary>
public class RisingPlatform : Obstacle
{
    [Header("Mécanique de la plateforme")]
    [SerializeField] private float overDrive = 1f;
    [SerializeField] private float vitesse = 5f;
    [SerializeField] private float hauteur = 4f;

    [Header("Debug")] [SerializeField] private bool debugLogs = false;

    private void OnEnable()  => unlocked += OnUnlock;
    private void OnDisable() => unlocked -= OnUnlock;

    /// <summary>
    /// Callback lors du déverrouillage de la plateforme.
    /// </summary>
    private void OnUnlock()
    {
        float targetY = transform.position.y + hauteur;
        float duration = hauteur / (vitesse * overDrive);
        transform.DOMoveY(targetY, duration).SetEase(Ease.OutQuad);
        if (debugLogs)
            Debug.Log($"[RisingPlatform] OnUnlock: targetY={targetY}, duration={duration}", this);
    }
}