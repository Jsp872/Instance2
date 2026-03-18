using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum NoteID : byte
{
    DO = 0,
    RE = 1,
    MI = 2,
    FA = 3,

    NONE = 255,
}
public class NoteHoldContext
{
    public NoteID id;
    public float holdTimer;
    public float startHoldTimer;
    public float timeToWaitForHold;
    public bool holdStarted;
    public bool CanUpdateHold => startHoldTimer >= timeToWaitForHold;

    public NoteHoldContext(NoteID id, float timeToWaitForHold)
    {
        this.id = id;
        this.timeToWaitForHold = timeToWaitForHold;
        holdTimer = 0f;
        startHoldTimer = 0f;
    }
}

public class SendNoteComponent : PlayerComponent
{
    [Header("Config")]
    [SerializeField] private SendNoteConfig config;
    private Coroutine comboRoutine;

    [Header("Hold helper")]
    private List<NoteContext> noteBuffer = new List<NoteContext>();
    private Dictionary<NoteID, NoteHoldContext> holdDurations = new Dictionary<NoteID, NoteHoldContext>();
    private HashSet<NoteID> holdingNotes = new HashSet<NoteID>();


    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        this.config = controller.GetConfig().sendNoteConfig;
    }

    public override void UpdateComponent(ref Vector3 velocity, float fixedDT)
    {
        foreach (NoteID note in holdingNotes.ToArray())
        {
            NoteHoldContext holdCtx = holdDurations[note];

            if (!holdCtx.holdStarted)
            {
                holdCtx.startHoldTimer += fixedDT;

                if (holdCtx.CanUpdateHold)
                {
                    holdCtx.holdStarted = true;

                    if (comboRoutine != null)
                    {
                        StopCoroutine(comboRoutine);
                        comboRoutine = null;
                    }
                }
            }
            else
            {
                holdCtx.holdTimer += config.holdTimerMultiplier * fixedDT;
            }
        }
    }

    public override void HandleInput<T>(InputAction.CallbackContext ctx, T param)
    {
        if (param is NoteID id)
        {
            if (ctx.started)
            {
                StartNote(id);
            }

            if (ctx.canceled)
            {
                ReleaseNote(id);
            }
        }
    }
    private void StartNote(NoteID id)
    {
        holdingNotes.Add(id);
        holdDurations[id] = new NoteHoldContext(id, config.isHoldDelay);

        noteBuffer.Add(new NoteContext(id, 0f));

        if (comboRoutine != null)
            StopCoroutine(comboRoutine);

        comboRoutine = StartCoroutine(ComboWindow());
    }
    private void ReleaseNote(NoteID id)
    {
        if (!holdingNotes.Contains(id))
            return;

        holdingNotes.Remove(id);

        if (!holdDurations.ContainsKey(id))
            return;

        var holdCtx = holdDurations[id];
        float holdTime = holdCtx.holdTimer;

        bool isHold = holdCtx.holdStarted;

        for (int i = 0; i < noteBuffer.Count; i++)
        {
            if (noteBuffer[i].note == id)
            {
                noteBuffer[i] = new NoteContext(id, holdTime);
            }
        }

        if (isHold)
        {
            EventBus.Publish(new NoteContext(id, holdTime));

            noteBuffer.RemoveAll(n => n.note == id);
        }

        holdDurations.Remove(id);
    }

    private IEnumerator ComboWindow()
    {
        yield return new WaitForSeconds(config.combosDelay);

        ProcessBuffer();
    }

    private void ProcessBuffer()
    {
        if (noteBuffer.Count == 0)
            return;

        List<NoteContext> toSend = new List<NoteContext>();
        foreach (var note in noteBuffer)
        {
            if (holdDurations.ContainsKey(note.note))
            {
                NoteHoldContext holdCtx = holdDurations[note.note];

                if (holdCtx.holdStarted)
                {
                    continue;
                }
            }

            toSend.Add(note);
        }

        Dictionary<NoteID, NoteContext> grouped = new();

        foreach (var note in toSend)
        {
            if (grouped.ContainsKey(note.note))
            {
                NoteContext existing = grouped[note.note];
                existing.noteSendCount += 1;

                existing.holdDuration = Mathf.Max(existing.holdDuration, note.holdDuration);

                grouped[note.note] = existing;
            }
            else
            {
                grouped[note.note] = new NoteContext(note.note, note.holdDuration, 1);
            }
        }

        foreach (var kvp in grouped)
        {
            EventBus.Publish(kvp.Value);
        }

        noteBuffer.RemoveAll(n => toSend.Contains(n));
    }
}