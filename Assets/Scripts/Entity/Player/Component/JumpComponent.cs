using UnityEngine;
using UnityEngine.InputSystem;

public class JumpComponent : PlayerComponent
{
    [SerializeField, Tooltip("Config Copy - changes don't save to SO!")]
    private JumpConfig configCopy;

    [Header("______DEBUG_______")]
    [SerializeField] private float coyoteTimer;
    [SerializeField] private bool wasGrounded;
    [SerializeField] private float jumpBufferTimer;
    [SerializeField] private float DEBUG_derivedJumpVelocity;
    [SerializeField] private float DEBUG_derivedGravity;

    private Rigidbody2D rb;
    private GroundSensor _groundSensor;

    private float derivedJumpVelocity;
    private float derivedGravity;

    private bool isJumping;
    private bool apexReached;
    private bool isFalling;

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        configCopy = controller.GetConfig().jumpConfig;
        rb = controller.GetRb();
        _groundSensor = controller.groundSensor;

        ComputeJumpPhysics();
    }

    private void ComputeJumpPhysics()
    {
        derivedJumpVelocity = (2f * configCopy.jumpApexHeight) / configCopy.jumpApexTime;
        derivedGravity = (2f * configCopy.jumpApexHeight) / (configCopy.jumpApexTime * configCopy.jumpApexTime);
        rb.gravityScale = derivedGravity / Mathf.Abs(Physics2D.gravity.y);

        DEBUG_derivedJumpVelocity = derivedJumpVelocity;
        DEBUG_derivedGravity = derivedGravity;
    }

    public override void HandleInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            jumpBufferTimer = configCopy.jumpBufferTime;

        //if (ctx.canceled && isJumping && rb.linearVelocityY > 0f)
        //    OnJumpCut();
    }

    public override void UpdateComponent(ref Vector3 velocity, float dt)
    {
        bool grounded = _groundSensor.IsGrounded;

        UpdateCoyoteTime(grounded, dt);
        UpdateLanding(grounded);

        wasGrounded = grounded;

        UpdateJumpBuffer(dt);

        if (configCopy.hasJumpGravityModifiers)
            UpdateGravity();

        if (isJumping)
        {
            UpdateApex();
            UpdateFall();
        }
    }

    private void UpdateCoyoteTime(bool grounded, float dt)
    {
        if (grounded)
        {
            coyoteTimer = configCopy.coyoteTime;
        }
        else if (wasGrounded && !grounded)
        {
            coyoteTimer = configCopy.hasCoyoteTime ? configCopy.coyoteTime : 0f;
        }
        else
        {
            coyoteTimer -= dt;
        }
    }

    private void UpdateJumpBuffer(float dt)
    {
        if (jumpBufferTimer <= 0f) return;
        jumpBufferTimer -= dt;
        if (CanJump())
        {
            PerformJump();
            jumpBufferTimer = 0f;
        }
    }

    private void UpdateGravity()
    {
        float baseGravityScale = derivedGravity / Mathf.Abs(Physics2D.gravity.y);

        if (rb.linearVelocityY < 0f)
            rb.gravityScale = baseGravityScale * configCopy.fallGravityMultiplier;
        else if (rb.linearVelocityY > 0f)
            rb.gravityScale = baseGravityScale * configCopy.cutJumpGravityMultiplier;
        else
            rb.gravityScale = baseGravityScale;
    }

    private void UpdateApex()
    {
        if (apexReached) return;
        if (rb.linearVelocityY <= 0f)
        {
            apexReached = true;
            OnApexReached();
        }
    }

    private void UpdateFall()
    {
        if (isFalling) return;
        if (rb.linearVelocityY < 0f)
        {
            isFalling = true;
            OnFallStarted();
        }
    }

    private void UpdateLanding(bool grounded)
    {
        if (!wasGrounded && grounded)
            OnLanded();
    }

    private bool CanJump() => coyoteTimer > 0f;

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, derivedJumpVelocity);
        coyoteTimer = 0f;
        isJumping = true;
        apexReached = false;
        isFalling = false;
        OnJumpStarted();
        EventBus.Publish(new JumpEvent());
    }

    private void OnJumpStarted()
    {
        Debug.Log("[Jump] Started");
        EventBus.Publish(new OnJumpStarted());
    }

    private void OnApexReached()
    {
        Debug.Log("[Jump] Apex reached");
        EventBus.Publish(new OnApexReached());
    }

    private void OnFallStarted()
    {
        Debug.Log("[Jump] Fall started");
        EventBus.Publish(new OnFallStarted());
    }

    private void OnLanded()
    {
        isJumping = false;
        apexReached = false;
        isFalling = false;
        EventBus.Publish(new OnJumpFinished());
    }
}