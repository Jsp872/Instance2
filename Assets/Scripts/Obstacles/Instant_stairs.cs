using UnityEngine;
using UnityEngine.PlayerLoop;

public class Instant_stairs : Stairs
{
    [SerializeField] private int PlatformNumber;

    void Awake()
    {
        InitPlatforms(PlatformNumber);
    }

    protected override void UnlockedBehaviour()
    {
        for (int i = 0; i < PlatformNumber; i++)
        {
            PlatformRise( 1);
            currentStep++;
        }
    }
}
