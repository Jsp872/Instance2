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

        SetAndInitComponent(out jumpComponent);
        SetAndInitComponent(out autoMoveComponent);
        SetAndInitComponent(out sendNoteComponent);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = UpdateComponent(autoMoveComponent, jumpComponent);
        transform.position += velocity * Time.fixedDeltaTime;
    }
    private void SetAndInitComponent<T>(out T component) where T : PlayerComponent
    {
        if (TryGetComponent(out component))
        {
            component.Initialize(playerConfig, rb);
        }
    }
    private Vector3 UpdateComponent(params PlayerComponent[] components)
    {
        Vector3 velocity = Vector2.zero;
        foreach (var component in components)
        {
            component.OnUpdated(ref velocity, Time.fixedDeltaTime);
        }
        return velocity;
    }

}