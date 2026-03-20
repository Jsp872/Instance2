using UnityEngine;

public class Progressive_stairs : Stairs
{
    void Awake()
    {
        InitPlatforms(steps);
    }

    protected override void UnLockingBehaviour()
    {
        PlatformRise(1);
        currentStep++;
    }

    protected override void UnlockedBehaviour()
    {
        PlatformRise(1);
    }

    protected override void LockedBehaviour()
    {
        int step = currentStep;
        for (int i = 0; i < step; i++)
        {
            currentStep--;
            PlatformRise(-1);
        }
    }
}
