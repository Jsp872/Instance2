using System;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int VerticalVel = Animator.StringToHash("VerticalVel");
    private static readonly int Dead = Animator.StringToHash("dead");
    [SerializeField] private Animator animator;
    [SerializeField] private GroundSensor groundSensor;
    [SerializeField] private Rigidbody2D rb;

    private void OnEnable()
    {
        EventBus.Subscribe<OnJumpStarted>(OnJumpStarted);
        EventBus.Subscribe<OnHitObstacleCallback>(OnDead);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnJumpStarted>(OnJumpStarted);
        EventBus.Unsubscribe<OnHitObstacleCallback>(OnDead);
    }

    private void Update()
    {
        animator.SetBool(IsGrounded, groundSensor.IsGrounded);
        animator.SetFloat(VerticalVel, rb.linearVelocityY);
        if (rb.linearVelocityY < -.1f)
        {
            print("VerticalVel :" + rb.linearVelocityY);
        }
    }

    private void OnJumpStarted(OnJumpStarted obj)
    {
        animator.SetTrigger(Jump);
    }

    private void OnDead(OnHitObstacleCallback obj)
    {
        animator.SetBool(Dead, true);
    }
}