using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SendNoteComponent : PlayerComponent
{
    [Header("Config")]
    [SerializeField] private SendNoteConfig configCopy;

    private bool isHold;
    private Coroutine waitForHoldRoutine;
    private Coroutine waitForCombosRoutine;

    [Header("DEBUG_STRUCT")]
    [SerializeField] private NoteContext noteContext;
    [Space(5.0f)]
    [SerializeField] private NoteContext previous;

    public override void Initialize(PlayerStatConfig config, Rigidbody2D rb)
    {
        base.Initialize(config, rb);
       this.configCopy = config.sendNoteConfig;
    }

    private readonly Dictionary<string, NoteID> keys = new Dictionary<string, NoteID>()
    {
        {"H", NoteID.DO },
        {"J", NoteID.RE },
        {"K", NoteID.MI },
        {"L", NoteID.FA },
    };

    public void OnSendDO(InputAction.CallbackContext ctx)
    {
        OnSendNote(ctx);
    }

    public void OnSendRE(InputAction.CallbackContext ctx)
    {
        OnSendNote(ctx);
    }

    public void OnSendMI(InputAction.CallbackContext ctx)
    {
        OnSendNote(ctx);
    }

    public void OnSendFA(InputAction.CallbackContext ctx)
    {
        OnSendNote(ctx);
    }

    private void OnSendNote(InputAction.CallbackContext ctx)
    {
        NoteID id = keys[ctx.control.displayName];

        if (ctx.performed)
        {
            previous = noteContext;
            noteContext = new NoteContext(id, 0.0f);
            OnActionStarted();
        }

        if (ctx.canceled) { 
            OnActionCanceled();
        }
    }
    public override void OnActionStarted()
    {
        isHold = false;
        waitForHoldRoutine = StartCoroutine(WaitForUpdateHold());

        if (waitForCombosRoutine != null)
        {
            StopCoroutine(waitForCombosRoutine);
            waitForCombosRoutine = null;
            if (previous.note == noteContext.note)
            {
                noteContext.noteSendCount = previous.noteSendCount + 1;
            }
            else
            {
                //bypass timer
                SendAndReset(previous);
            }
        }
    }
    public override void OnActionCanceled()
    {
        StopAllCoroutines();
        if (isHold) { 
            SendAndReset(noteContext);
            previous = noteContext;
        }
        waitForCombosRoutine = StartCoroutine(WaitForSendNoteContext());
    }
    private void Update()
    {
        if (isHold)
        {
            noteContext.holdDuration += configCopy.holdTimerMultiplier * Time.deltaTime;
        }
    }

    private IEnumerator WaitForUpdateHold()
    {
        yield return new WaitForSeconds(configCopy.isHoldDelay);
        isHold = true;
    }
    private IEnumerator WaitForSendNoteContext()
    {
        yield return new WaitForSeconds(configCopy.combosDelay);
        SendAndReset(noteContext);
        previous = noteContext;
    }
    private void SendAndReset(NoteContext ctx)
    {
        EventBus.Publish(ctx);
        waitForCombosRoutine = null;
        waitForHoldRoutine = null;
        isHold = false;
    }
}

public enum NoteID : byte
{
    DO = 0,
    RE = 1,
    MI = 2,
    FA = 3,

    NONE = 255,
}