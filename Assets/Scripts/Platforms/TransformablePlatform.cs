using UnityEngine;

/// <summary>
/// Plateforme transformable activée par une note ou un saut.
/// </summary>
public class TransformablePlatform : BasePlatform
{
    private enum TriggerType { Jump, Note }

    [Header("Activation Settings")]
    [SerializeField] private TriggerType triggerType;
    [SerializeField] private NoteID targetNote;

    [Header("Initial State")]
    [SerializeField] private bool startActive = true;

    [Header("Debug")] [SerializeField] private bool debugLogs = false;

    [SerializeField] private Renderer _renderer;
    
    private bool _isActive;
    private Collider2D _collider;

    /// <summary>
    /// Initialisation de la plateforme.
    /// </summary>
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        SetState(startActive);
        if (debugLogs)
            Debug.Log($"[TransformablePlatform] Awake: startActive={startActive}", this);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<JumpEvent>(OnJump);
        EventBus.Subscribe<NoteID>(OnNote);
        if (debugLogs)
            Debug.Log("[TransformablePlatform] OnEnable: Subscribed to events.", this);
    }

    private void OnNote(NoteID receivedNote)
    {
        if (triggerType == TriggerType.Note && receivedNote == targetNote)
        {
            ToggleState();
            if (debugLogs)
                Debug.Log($"[TransformablePlatform] OnNote: Note reçue {receivedNote}, ToggleState.", this);
        }
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<JumpEvent>(OnJump);
        EventBus.Unsubscribe<NoteID>(OnNote);
        if (debugLogs)
            Debug.Log("[TransformablePlatform] OnDisable: Unsubscribed from events.", this);
    }

    protected override void OnPlayerEnter(PlayerController player)
    {
        if (debugLogs)
            Debug.Log($"[TransformablePlatform] OnPlayerEnter: {player.name}", this);
    }

    private void OnJump(JumpEvent evt)
    {
        if (triggerType == TriggerType.Jump)
        {
            ToggleState();
            if (debugLogs)
                Debug.Log("[TransformablePlatform] OnJump: ToggleState.", this);
        }
    }
    private void ToggleState()
    {
        SetState(!_isActive);
        if (debugLogs)
            Debug.Log($"[TransformablePlatform] ToggleState: _isActive={_isActive}", this);
    }

    private void SetState(bool active)
    {
        _isActive = active;
        if (_collider != null) _collider.enabled = active;
        if (_renderer != null) _renderer.enabled = active;
        if (debugLogs)
            Debug.Log($"[TransformablePlatform] SetState: active={active}", this);
    }
}
