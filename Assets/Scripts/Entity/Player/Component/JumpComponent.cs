using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpComponent : PlayerComponent
{
    [Header("DEBUG")]
    [SerializeField] private JumpConfig configCopy;


    [SerializeField] private Transform playerFeet;

    private float additionalForce;
    private bool holdJump = false;
    private Coroutine holdJumpRoutine;

    public override void Initialize(PlayerStatConfig config, Rigidbody2D rb)
    {
        base.Initialize(config, rb);
        configCopy = playerConfig.jumpConfig;
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


        additionalForce += fixedDeltaTime * configCopy.jumpAcceleration;
        additionalForce = Mathf.Clamp(
            additionalForce,
            0f,
            configCopy.maximalJumpForce - configCopy.initialJumpForce
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
            configCopy.initialJumpForce + additionalForce,
            configCopy.minimalJumpForce,
            configCopy.maximalJumpForce
        );

        rb.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
        additionalForce = 0f;
    }

    private bool IsGrounded()
    {
        bool isGrounded = Physics2D.Raycast(
            playerFeet.position,
            Vector2.down,
            configCopy.checkIsGroundedRadius,
            configCopy.jumpableLayer
        );
        Debug.DrawLine(playerFeet.position, playerFeet.position + Vector3.down * configCopy.checkIsGroundedRadius, Color.red);


        return isGrounded;
    }

    private IEnumerator WaitForHoldJump()
    {
        yield return new WaitForSeconds(configCopy.holdJumpStartTime);
        holdJump = true;
    }

    public void OnValidate()
    {
    //    movementConfig.moveSpeedDefaultValue =
    //        Mathf.Clamp(movementConfig.moveSpeedDefaultValue, movementConfig.minMoveSpeed, movementConfig.maxMoveSpeed);

        configCopy.initialJumpForce =
            Mathf.Clamp(configCopy.initialJumpForce, configCopy.minimalJumpForce, configCopy.maximalJumpForce);
    }
}