using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Stairs : Movement_obstacle
{
    [SerializeField] private GameObject Platform;
    [SerializeField] private float spacing;
    protected List<GameObject> Platforms = new List<GameObject>();
    protected int steps;
    protected int currentStep;

    protected void InitPlatforms(int number)
    {
        steps = sequenceCible.Count;
        for (int i = 0; i < number; i++)
        {
            Vector2 pos = new Vector2(transform.position.x + spacing * i, transform.position.y);
            GameObject newPlatform = Instantiate(Platform, pos, Quaternion.identity);
            Platforms.Add(newPlatform);
            print(newPlatform);
        }
    }

    protected void PlatformRise()
    {
        Platforms[currentStep].transform.DOMoveY(transform.position.y + distance/steps * (currentStep+1) , duration/Platforms.Count * (currentStep+1)).SetEase(Ease.InQuad);
    }

    protected void PlatformFall()
    {
        Platforms[currentStep].transform.DOMoveY(transform.position.y, duration/steps* (currentStep+1)).SetEase(Ease.InQuad);
    }
}
