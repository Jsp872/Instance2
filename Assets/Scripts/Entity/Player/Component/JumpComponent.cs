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
    [SerializeField] private float DEBUG_derivedJumpVelocity;
    [SerializeField] private float DEBUG_derivedGravity;

    private Rigidbody2D rb;
    private float derivedJumpVelocity;
    private float derivedGravity;

    private bool isJumping;
    private bool apexReached;
    private bool isFalling;

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        config = controller.GetConfig().jumpConfig;
        rb = controller.GetRb();
        ComputeJumpPhysics();
    }

    private void ComputeJumpPhysics()
    {
        derivedJumpVelocity = (2f * config.jumpApexHeight) / config.jumpApexTime;
        derivedGravity = (2f * config.jumpApexHeight) / (config.jumpApexTime * config.jumpApexTime);
        rb.gravityScale = derivedGravity / Mathf.Abs(Physics2D.gravity.y);
        DEBUG_derivedJumpVelocity = derivedJumpVelocity;
        DEBUG_derivedGravity = derivedGravity;
    }

    public override void HandleInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            jumpBufferTimer = config.jumpBufferTime;

        //if (ctx.canceled && isJumping && rb.linearVelocityY > 0f)
        //    OnJumpCut();
    }

    public override void UpdateComponent(ref Vector3 velocity, float dt)
    {
        bool grounded = IsGrounded();

        UpdateCoyoteTime(grounded, dt);
        UpdateLanding(grounded);

        wasGrounded = grounded;

        UpdateJumpBuffer(dt);

        if (config.hasJumpGravityModifiers)
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
            coyoteTimer = config.coyoteTime;
        }
        else if (wasGrounded && !grounded)
        {
            coyoteTimer = config.hasCoyoteTime ? config.coyoteTime : 0f;
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
            rb.gravityScale = baseGravityScale * config.fallGravityMultiplier;
        else if (rb.linearVelocityY > 0f)
            rb.gravityScale = baseGravityScale * config.cutJumpGravityMultiplier;
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
        // SFX saut, squash animation, particules sol
        Debug.Log("[Jump] Started");
        EventBus.Publish(new OnJumpStarted());
    }

    //private void OnJumpCut()
    //{
    //    // Feedback visuel : saut coupé (ex: particule burst vers le bas)
    //    Debug.Log("[Jump] Cut");
    //}

    private void OnApexReached()
    {
        // Freeze frame court, particules apex, sfx apex
        Debug.Log("[Jump] Apex reached");
        EventBus.Publish(new OnApexReached());
    }

    private void OnFallStarted()
    {
        // Animation de chute, sfx wind, tilt du sprite
        Debug.Log("[Jump] Fall started");
        EventBus.Publish(new OnFallStarted());
    }

    private void OnLanded()
    {
        isJumping = false;
        apexReached = false;
        isFalling = false;
        // Squash landing, dust particles, sfx impact, screen shake
        Debug.Log("[Jump] Landed");
        EventBus.Publish(new OnjumpFinished());
    }

    private bool IsGrounded()
    {

        RaycastHit2D hit = MultyRaycastUtils.MultiRaycast(
            origin: playerFeet,
            direction: Vector2.down,
            distance: config.checkIsGroundedRadius,
            count: config.groundedRaycastCount,
            spreadAxis: Vector2.right,
            spread: config.raycastOffset,
            layerMask: playerController.collisionLayers
        );


        return hit;
    }
}