using DG.Tweening;
using UnityEngine;

/// <summary>
/// Plateforme à piques qui descend lors du déverrouillage.
/// </summary>
public class Spike : Moving_obstacle
{



    /// <summary>
    /// Callback lors du déverrouillage des piques.
    /// </summary>
    protected override void UnlockedBehaviour()
    {
        transform.DOMoveY(transform.position.y - distance, duration).SetEase(Ease.InQuad);
        if (debugLogs)
            Debug.Log($"[Spike] OnUnlocked: distance={distance}, duration={duration}", this);
    }
}