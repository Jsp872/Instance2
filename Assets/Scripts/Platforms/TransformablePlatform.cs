using UnityEngine;

public class TransformablePlatform : BasePlatform
{
    private enum TriggerType { Jump, Note }

    [Header("Activation Settings")]
    [SerializeField] private TriggerType triggerType;
    [SerializeField] private InputEnum targetNote;

    [Header("Initial State")]
    [SerializeField] private bool startActive = true;

    private bool _isActive;
    private Collider2D _collider;
    private Renderer _renderer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<Renderer>();
        SetState(startActive);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<JumpEvent>(OnJump);
        EventBus.Subscribe<NoteEvent>(OnNote);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<JumpEvent>(OnJump);
        EventBus.Unsubscribe<NoteEvent>(OnNote);
    }

    protected override void OnPlayerEnter(PlayerController player) { }

    private void OnJump(JumpEvent evt)
    {
        if (triggerType == TriggerType.Jump) ToggleState();
    }

    private void OnNote(NoteEvent evt)
    {
        if (triggerType == TriggerType.Note && evt.Note == targetNote) ToggleState();
    }

    private void ToggleState() => SetState(!_isActive);

    private void SetState(bool active)
    {
        _isActive = active;
        if (_collider != null) _collider.enabled = active;
        if (_renderer != null) _renderer.enabled = active;
    }
}
