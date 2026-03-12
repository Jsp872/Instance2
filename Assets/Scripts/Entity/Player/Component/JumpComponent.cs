using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpComponent : PlayerComponent
{
    [SerializeField] private Transform playerFeet;

    private float additionalForce;
    private bool holdJump = false;
    private Coroutine holdJumpRoutine;
    private float accel;

    [SerializeField, Tooltip("Set this var on SO config")] private float holdJumpStartTime = 0.025f;
    [SerializeField, Tooltip("Set this var on SO config")] private float checkIsGroundedRadius = 0.25f;

    public override void Initialize(PlayerStatConfig config, Rigidbody2D rb)
    {
        base.Initialize(config, rb);
        accel = playerConfig.jumpAcceleration;
    }


    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (IsGrounded())
        {
            if (ctx.started) {
                OnActionStarted();
            }

            else if (ctx.canceled) {
                OnActionCanceled();
            }
        }
    }
    public override void OnActionStarted()
    {
        base.OnActionStarted();
        holdJumpRoutine = StartCoroutine(WaitForHoldJump());
    }
    public override void OnUpdated(ref Vector3 velocity, float fixedDeltaTime)
    {
        if (!holdJump) return;


        additionalForce += fixedDeltaTime * accel;
        additionalForce = Mathf.Clamp(
            additionalForce,
            0f,
            playerConfig.maximalJumpForce - playerConfig.initialJumpForce
        );

    }
    public override void OnActionCanceled()
    {
        base.OnActionCanceled();
        holdJump = false;

        if (holdJumpRoutine != null)
        {
            StopCoroutine(holdJumpRoutine);
            holdJumpRoutine = null;
        }

        TryJump();
    }

    private void TryJump()
    {
        if (!IsGrounded()) return;

        float force = Mathf.Clamp(
            playerConfig.initialJumpForce + additionalForce,
            playerConfig.minimalJumpForce,
            playerConfig.maximalJumpForce
        );

        rb.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
        additionalForce = 0f;
    }

    private bool IsGrounded()
    {
        bool isGrounded = Physics2D.Raycast(
            playerFeet.position,
            Vector2.down,
            checkIsGroundedRadius,
            playerConfig.groundLayer | playerConfig.jumpableLayer
        );
        Debug.DrawLine(playerFeet.position, playerFeet.position + Vector3.down * checkIsGroundedRadius, Color.red);


        return isGrounded;
    }

    private IEnumerator WaitForHoldJump()
    {
        yield return new WaitForSeconds(holdJumpStartTime);
        holdJump = true;
    }
}