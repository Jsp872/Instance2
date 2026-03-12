using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStatConfig playerConfig;

    private JumpComponent jumpComponent;
    private AutoMoveComponent autoMoveComponent;
    private SendNoteComponent sendNoteComponent;
    private Rigidbody2D rb;


    #region SEND_NOTE_DEBUG
    private void OnEnable()
    {
        EventBus.Subscribe<SendNoteCallback>(NoteReceived);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<SendNoteCallback>(NoteReceived);
    }

    private void NoteReceived(SendNoteCallback callback)
    {
        print($"note received : \n" +
            $"type -> {callback.note} \n" +
            $"count -> {callback.noteSendCount} \n" +
            $"holdDuration -> {callback.holdDuration}.");
    }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        jumpComponent = GetComponent<JumpComponent>();
        sendNoteComponent = GetComponent<SendNoteComponent>();
        autoMoveComponent = GetComponent<AutoMoveComponent>();

        jumpComponent.Initialize(playerConfig, rb);
        autoMoveComponent.Initialize(playerConfig, rb);
        sendNoteComponent.Initialize(playerConfig, rb);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        autoMoveComponent.OnUpdated(ref velocity, Time.fixedDeltaTime);
        jumpComponent.OnUpdated(ref velocity, Time.fixedDeltaTime);
        //sendNoteComponent.OnUpdated(ref velocity, Time.fixedDeltaTime);

        transform.position += velocity * Time.fixedDeltaTime;
    }
}