using DG.Tweening;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private float duration;
    
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
        transform.DOMoveY(transform.position.y - distance, duration).SetEase(Ease.InQuad);
    }

    private void OnDisable()
    {
        obstaclePlayerDetector.unlocked -= OnObstaclePlayerDetectorUnlocked;
    }
}
