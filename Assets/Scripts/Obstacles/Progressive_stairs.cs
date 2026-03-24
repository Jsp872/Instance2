using UnityEngine;

public class Progressive_stairs : Stairs
{
    private void Start()
    {
        InitPlatforms(sequenceCible.Count);
    }

    protected override void UnLockingBehaviour()
    {
        PlatformRise();
        currentStep++;
    }

    protected override void UnlockedBehaviour()
    {
        PlatformRise();
        base.UnlockedBehaviour();
    }

    protected override void LockedBehaviour()
    {
        int step = currentStep;
        for (int i = 0; i < step; i++)
        {           
            currentStep--;
            PlatformFall();
        }
    }
}
