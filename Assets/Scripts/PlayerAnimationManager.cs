using System;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Land = Animator.StringToHash("Land");
    [SerializeField] private Animator animator;
    private void OnEnable()
    {
        EventBus.Subscribe<OnJumpStarted>(OnJumpStarted);
        EventBus.Subscribe<OnApexReached>(OnApexReached);
        EventBus.Subscribe<OnFallStarted>(OnFallStarted);
        EventBus.Subscribe<OnJumpFinished>(OnJumpFinished);
    }

    private void OnDisable()
    {
         EventBus.Unsubscribe<OnJumpStarted>(OnJumpStarted);
         EventBus.Unsubscribe<OnApexReached>(OnApexReached);
         EventBus.Unsubscribe<OnFallStarted>(OnFallStarted);
         EventBus.Unsubscribe<OnJumpFinished>(OnJumpFinished);
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
}
