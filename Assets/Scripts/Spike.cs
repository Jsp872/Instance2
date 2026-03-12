using DG.Tweening;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private ObstaclePlayerDetector obstaclePlayerDetector;

    private void Awake()
    {
        obstaclePlayerDetector = GetComponentInChildren<ObstaclePlayerDetector>();
    }

    private void OnEnable()
    {
        obstaclePlayerDetector.unlocked += OnObstaclePlayerDetectorUnlocked;
    }

    private void OnObstaclePlayerDetectorUnlocked()
    {
        transform.DOMoveY(transform.position.y - 5f, 0.5f).SetEase(Ease.InQuad);
    }

    private void OnDisable()
    {
        obstaclePlayerDetector.unlocked -= OnObstaclePlayerDetectorUnlocked;
    }
}
