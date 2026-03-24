using System;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Land = Animator.StringToHash("Land");
    private static readonly int Explode = Animator.StringToHash("Explode");
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        EventBus.Subscribe<OnJumpStarted>(OnJumpStarted);
        EventBus.Subscribe<OnApexReached>(OnApexReached);
        EventBus.Subscribe<OnFallStarted>(OnFallStarted);
        EventBus.Subscribe<OnJumpFinished>(OnJumpFinished);
        EventBus.Subscribe<OnHitObstacleCallback>(OnDead);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnJumpStarted>(OnJumpStarted);
        EventBus.Unsubscribe<OnApexReached>(OnApexReached);
        EventBus.Unsubscribe<OnFallStarted>(OnFallStarted);
        EventBus.Unsubscribe<OnJumpFinished>(OnJumpFinished);
        EventBus.Unsubscribe<OnHitObstacleCallback>(OnDead);
    }

    private void OnJumpFinished(OnJumpFinished obj)
    {
        animator.SetTrigger(Land);
    }

    private void OnFallStarted(OnFallStarted obj)
    {
        animator.SetTrigger(Fall);
    }

    private void OnApexReached(OnApexReached obj)
    {
    }

    private void OnJumpStarted(OnJumpStarted obj)
    {
        animator.SetTrigger(Jump);
    }

    private void OnDead(OnHitObstacleCallback obj)
    {
        float respawnDelay = GetComponent<Player>().GetRespawnDelay;
        float deathAnimDuration = 0.3f;

        animator.speed = deathAnimDuration / respawnDelay;
        animator.SetTrigger(Explode);
    }
}