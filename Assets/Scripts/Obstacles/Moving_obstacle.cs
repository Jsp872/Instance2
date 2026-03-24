using UnityEngine;
using DG.Tweening;

public class Moving_obstacle : Movement_obstacle
{
    private int steps;
    private int currentSteps;

    void Start()
    {
        steps = sequenceCible.Count;
    }
    
    protected override void UnlockedBehaviour()
    {
        transform.DOMoveY(transform.position.y - distance/steps, duration/steps).SetEase(Ease.InQuad);
        if (debugLogs)
            Debug.Log($"[Spike] OnUnlocked: distance={distance}, duration={duration}", this);

        base.UnlockedBehaviour();
    }

    protected override void UnLockingBehaviour()
    {
        currentSteps++;
        transform.DOMoveY(transform.position.y - distance/steps, duration/steps).SetEase(Ease.InQuad);
    }

    protected override void LockedBehaviour()
    {
        transform.DOMoveY(transform.position.y + distance/steps*currentSteps, duration/steps*currentSteps).SetEase(Ease.InQuad);
    }
}
