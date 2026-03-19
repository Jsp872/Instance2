using UnityEngine;
using UnityEngine.InputSystem;

// Contrôleur principal du joueur, orchestre les composants modulaires et gère l'entrée.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Permet d'activer/désactiver les logs de debug depuis l'inspecteur.
    [Header("Debug")][SerializeField] private bool debugLogs = false;

    private PlayerStatConfig playerConfig;
    private Rigidbody2D rb;


    public PlayerStatConfig GetConfig() => playerConfig;
    public Rigidbody2D GetRb() => rb;

    [Header("InputComponent")]
    private JumpComponent jumpComponent;
    private SendNoteComponent sendNoteComponent;
    private PauseComponent pauseComponent;

    [Header("MovementComponent")]
    private AutoMoveComponent autoMoveComponent;

    /// <summary>
    /// Initialise tous les composants du joueur.
    /// </summary>
    public void InitializeComponent(PlayerStatConfig playerStatConfig)
    {
        playerConfig = playerStatConfig;
        AddAndInitComponent(out rb);

        AddAndInitComponent(out jumpComponent);
        AddAndInitComponent(out sendNoteComponent);
        AddAndInitComponent(out pauseComponent);

        AddAndInitComponent(out autoMoveComponent);
        if (debugLogs)
            Debug.Log("[PlayerController] Composants initialisés.", this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision?.gameObject.name);
        if (collision.gameObject.layer == playerConfig.spikeLayer)
        {
            print("collision avec le skipe");
            EventBus.Publish(new OnHitObstacleCallback());
        }
    }

    /// <summary>
    /// Applique une force d'impulsion pour le rebond.
    /// </summary>
    public void Bounce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
        if (debugLogs)
            Debug.Log($"[PlayerController] Bounce: force={force}", this);
    }

    private void Update()
    {
        Vector3 velocity = Vector3.zero;
        UpdateComp(ref velocity, Time.deltaTime, autoMoveComponent, jumpComponent);
        rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocityY);
        if (debugLogs)
            Debug.Log($"[PlayerController] Update: velocity={velocity}", this);
    }

    #region Input Controller
    // Gestion des entrées utilisateur pour chaque note et action.
    public void OnSendDO(InputAction.CallbackContext ctx) { if (debugLogs) Debug.Log("[PlayerController] OnSendDO", this); sendNoteComponent.HandleInput(ctx, NoteID.DO); }
    public void OnSendRE(InputAction.CallbackContext ctx) { if (debugLogs) Debug.Log("[PlayerController] OnSendRE", this); sendNoteComponent.HandleInput(ctx, NoteID.RE); }
    public void OnSendMI(InputAction.CallbackContext ctx) { if (debugLogs) Debug.Log("[PlayerController] OnSendMI", this); sendNoteComponent.HandleInput(ctx, NoteID.MI); }
    public void OnSendFA(InputAction.CallbackContext ctx) { if (debugLogs) Debug.Log("[PlayerController] OnSendFA", this); sendNoteComponent.HandleInput(ctx, NoteID.FA); }
    public void OnJump(InputAction.CallbackContext ctx) { if (debugLogs) Debug.Log("[PlayerController] OnJump", this); jumpComponent.HandleInput(ctx); }
    public void OnOpenPauseSetting(InputAction.CallbackContext ctx) { if (debugLogs) Debug.Log("[PlayerController] OnOpenPauseSetting", this); pauseComponent.HandleInput(ctx); }
    #endregion

    /// <summary>
    /// Ajoute et initialise un composant sur le GameObject.
    /// </summary>
    private void AddAndInitComponent<T>(out T component) where T : Component
    {
        if (!TryGetComponent(out component))
        {
            component = gameObject.AddComponent<T>();
            if (debugLogs)
                Debug.Log($"[PlayerController] Composant ajouté: {typeof(T).Name}", this);
        }

        if (component is PlayerComponent pComponent)
        {
            pComponent.Initialize(this);
            if (debugLogs)
                Debug.Log($"[PlayerController] PlayerComponent initialisé: {typeof(T).Name}", this);
        }
    }

    /// <summary>
    /// Met à jour tous les composants du joueur.
    /// </summary>
    private void UpdateComp(ref Vector3 velocity, float fixedDT, params PlayerComponent[] components)
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].UpdateComponent(ref velocity, fixedDT);
            if (debugLogs)
                Debug.Log($"[PlayerController] UpdateComp: {components[i].GetType().Name}", this);
        }
    }
}