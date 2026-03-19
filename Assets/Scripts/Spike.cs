using DG.Tweening;
using UnityEngine;

/// <summary>
/// Plateforme à piques qui descend lors du déverrouillage.
/// </summary>
public class Spike : Obstacle
{
    [SerializeField] private float distance;
    [SerializeField] private float duration;

    private void OnEnable()  => unlocked += OnUnlocked;
    private void OnDisable() => unlocked -= OnUnlocked;

    /// <summary>
    /// Callback lors du déverrouillage des piques.
    /// </summary>
    private void OnUnlocked()
    {
        transform.DOMoveY(transform.position.y - distance, duration).SetEase(Ease.InQuad);
        if (debugLogs)
            Debug.Log($"[Spike] OnUnlocked: distance={distance}, duration={duration}", this);
    }
}