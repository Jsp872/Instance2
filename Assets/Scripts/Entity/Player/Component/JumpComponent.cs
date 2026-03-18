using UnityEngine;
using UnityEngine.InputSystem;

public class JumpComponent : PlayerComponent
{
    [SerializeField] private Transform playerFeet;
    [SerializeField, Tooltip("Config Cache - changes don't save to SO!")]
    private JumpConfig config;

    [Header("______DEBUG_______")]
    [SerializeField] private float coyoteTimer;
    [SerializeField] private bool wasGrounded;
    [SerializeField] private float jumpBufferTimer;
    [SerializeField] private bool holdingJump;
    [SerializeField] private float totalForceApplied;


    [SerializeField] private float DEBUG_JumpForce;

    private Rigidbody2D rb;

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        config = controller.GetConfig().jumpConfig;
        rb = controller.GetRb();
    }

    public override void HandleInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jumpBufferTimer = config.jumpBufferTime;
        }
        else if (ctx.canceled)
        {
            holdingJump = false;
        }
    }

    public override void UpdateComponent(ref Vector3 velocity, float dt)
    {
        bool grounded = IsGrounded();

        if (grounded) coyoteTimer = config.coyoteTime;
        else if (wasGrounded && !grounded) coyoteTimer = config.coyoteTime;
        else coyoteTimer -= dt;
        wasGrounded = grounded;

        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= dt;
            if (CanJump())
            {
                PerformJump();
                jumpBufferTimer = 0f;
            }
        }

        if (holdingJump && rb.linearVelocityY > 0f && totalForceApplied < config.maximalJumpForce)
        {
            DEBUG_JumpForce = config.holdForcePerSecond * dt;
            DEBUG_JumpForce = Mathf.Min(DEBUG_JumpForce, config.maximalJumpForce - totalForceApplied);
            rb.AddForce(Vector2.up * DEBUG_JumpForce, ForceMode2D.Impulse);
            totalForceApplied += DEBUG_JumpForce;
        }

        if (rb.linearVelocityY < 0f)
            rb.gravityScale = config.fallGravityMultiplier;
        else if (rb.linearVelocityY > 0f && !holdingJump)
            rb.gravityScale = config.cutJumpGravityMultiplier;
        else
            rb.gravityScale = 1f;
    }

    private bool CanJump() => coyoteTimer > 0f;

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        playerController.Bounce(new Vector2(0f, config.minimalJumpForce));
        totalForceApplied = config.minimalJumpForce;
        holdingJump = true; 
        coyoteTimer = 0f;
        EventBus.Publish(new JumpEvent());
    }

    private bool IsGrounded()
    {
        bool hit = Physics2D.Raycast(
            playerFeet.position,
            Vector2.down,
            config.checkIsGroundedRadius,
            config.jumpableLayer
        );
        Debug.DrawLine(playerFeet.position,
            playerFeet.position + Vector3.down * config.checkIsGroundedRadius, Color.red);
        return hit;
    }
}