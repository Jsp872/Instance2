using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Stairs : Movement_obstacle
{
    [SerializeField] private GameObject Platform;
    [SerializeField] private float spacing;
    protected List<GameObject> Platforms;
    protected int steps;
    protected int currentStep;

    protected void InitPlatforms(int number)
    {
        steps = sequenceCible.Count;
        for (int i = 0; i < number; i++)
        {
            Vector2 pos = new Vector2(transform.position.x + spacing * i, transform.position.y);
            Platforms[i] = Instantiate(Platform, pos, Quaternion.identity);
        }
    }

    protected void PlatformRise(int direction)
    {
        Platforms[currentStep].transform.DOMoveY(duration/steps * (currentStep+1) * direction, duration/Platforms.Count * (currentStep+1)).SetEase(Ease.InQuad);
    }
}
