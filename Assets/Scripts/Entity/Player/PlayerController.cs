using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
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


    //PUBLIC API
    public void InitializeComponent(PlayerStatConfig playerStatConfig)
    {
        playerConfig = playerStatConfig;    
        AddAndInitComponent(out  rb);   

        AddAndInitComponent(out jumpComponent);
        AddAndInitComponent(out sendNoteComponent);
        AddAndInitComponent(out pauseComponent);

        AddAndInitComponent(out autoMoveComponent);
    }
    public void Bounce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
    private void Update()
    {
        Vector3 velocity = Vector3.zero;
        UpdateComp(ref velocity, Time.deltaTime, autoMoveComponent, jumpComponent);
        rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocityY);
    }

    #region SEND_NOTE_DEBUG
    private void OnEnable()
    {
        EventBus.Subscribe<NoteContext>(NoteReceived);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<NoteContext>(NoteReceived);
    }

    private void NoteReceived(NoteContext callback)
    {
        print($"note received : |--| " +
            $"type -> {callback.note} |--| " +
            $"count -> {callback.noteSendCount} |--| " +
            $"holdDuration -> {callback.holdDuration}.");
    }
    #endregion

    #region Input Controller
    public void OnSendDO(InputAction.CallbackContext ctx) => sendNoteComponent.HandleInput(ctx, NoteID.DO);
    public void OnSendRE(InputAction.CallbackContext ctx) => sendNoteComponent.HandleInput(ctx, NoteID.RE);
    public void OnSendMI(InputAction.CallbackContext ctx) => sendNoteComponent.HandleInput(ctx, NoteID.MI);
    public void OnSendFA(InputAction.CallbackContext ctx) => sendNoteComponent.HandleInput(ctx, NoteID.FA);
    public void OnJump(InputAction.CallbackContext ctx) => jumpComponent.HandleInput(ctx);

    public void OnOpenPauseSetting(InputAction.CallbackContext ctx) => pauseComponent.HandleInput(ctx);

    #endregion

    private void AddAndInitComponent<T>(out T component) where T : Component
    {
        if (!TryGetComponent(out component))
        {
            component = gameObject.AddComponent<T>();
        }

        if (component is PlayerComponent pComponent)
        {
            pComponent.Initialize(this);
        }
    }

    private void UpdateComp(ref Vector3 velocity, float fixedDT, params PlayerComponent[] components)
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].UpdateComponent(ref velocity, fixedDT);
        }
    }
}