using UnityEngine;
using UnityEngine.PlayerLoop;

public class Instant_stairs : Stairs
{
    [SerializeField] private int PlatformNumber;

    private void Start()
    { 
        InitPlatforms(PlatformNumber);
    }

    protected override void UnlockedBehaviour()
    {
        for (int i = 0; i < PlatformNumber; i++)
        {
            PlatformRise();
            currentStep++;
        }
        base.UnlockedBehaviour();
    }
}
