using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Runner automatique 2D.
/// - Se déplace vers la droite en continu à haute vitesse.
/// - Le joueur peut sauter (Space / bouton Sud gamepad) avec un saut court
///   ou long selon la durée d'appui (better-jump gravity).
///
/// Setup requis dans l'Inspector :
///   • Rigidbody2D (ajouté automatiquement)
///   • Un enfant "GroundCheck" positionné sous les pieds → glisser dans <groundCheck>
///   • Layer "Ground" sur les plateformes → sélectionner dans <groundLayer>
///   • Drag l'action "Player/Jump" de InputSystem_Actions dans <jumpAction>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    // ─── Mouvement ───────────────────────────────────────────────────────────

    [Header("Mouvement")]
    [SerializeField] private float speed = 14f;

    // ─── Saut ────────────────────────────────────────────────────────────────

    [Header("Saut")]
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float fallMultiplier = 3.5f;       // chute rapide
    [SerializeField] private float lowJumpMultiplier = 2.5f;    // saut court si on lâche tôt

    [Header("Détection du sol")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.12f;
    [SerializeField] private LayerMask groundLayer;

    // ─── Input ───────────────────────────────────────────────────────────────

    [Header("Input")]
    [SerializeField] private InputActionReference jumpAction;

    // ─── Privé ───────────────────────────────────────────────────────────────

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool jumpRequested;

    // ─── Lifecycle ───────────────────────────────────────────────────────────

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnEnable()
    {
        if (jumpAction == null) return;
        jumpAction.action.Enable();
        jumpAction.action.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        if (jumpAction == null) return;
        jumpAction.action.performed -= OnJumpPerformed;
        jumpAction.action.Disable();
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        // Mouvement horizontal automatique (écrase uniquement X)
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // Saut
        if (jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }

        // Better-jump gravity : chute rapide / saut court si relâché tôt
        ApplyBetterJump();
    }

    // ─── Sol ─────────────────────────────────────────────────────────────────

    private void CheckGround()
    {
        if (groundCheck == null) return;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // ─── Saut ────────────────────────────────────────────────────────────────

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (isGrounded && ctx.performed)
        {
            jumpRequested = true;
        }
    }

    private void ApplyBetterJump()
    {
        float gravityY = Physics2D.gravity.y; // négatif

        if (rb.linearVelocity.y < 0f)
        {
            // Phase descendante : gravité multipliée → chute franche
            rb.linearVelocity += Vector2.up * gravityY * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0f && !JumpIsHeld())
        {
            // Phase montante mais bouton relâché → gravité multipliée → saut court
            rb.linearVelocity += Vector2.up * gravityY * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private bool JumpIsHeld()
    {
        return jumpAction != null && jumpAction.action.IsPressed();
    }

    // ─── Gizmos (debug) ──────────────────────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
