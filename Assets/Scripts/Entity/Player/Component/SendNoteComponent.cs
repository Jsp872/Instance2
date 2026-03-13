using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SendNoteComponent : PlayerComponent
{
    private bool isHold;
    private Coroutine waitForHoldRoutine;
    private Coroutine waitForCombosRoutine;

    [SerializeField] private NoteContext noteContext;
    [SerializeField] private NoteContext previous;

    [Tooltip("quand le joueur appuie sur sa touche, attend ce delay avant de commencer a compté le hold de la touche [A METTRE DANS LE SendNoteConfig]")]
    [SerializeField] float isHoldDelay = 0.125f;

    [Tooltip("si deux touche identique sont jouer dans ce timing alors un combos est jouer [A METTRE DANS LE SendNoteConfig]")]
    [SerializeField] float combosDelay = 0.125f;

    [Tooltip("hold timer multiplicateur [A METTRE DANS LE SendNoteConfig]")]
    [SerializeField, Min(0.1f)] float holdTimerMultiplier;


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
            noteContext.holdDuration += holdTimerMultiplier * Time.deltaTime;
        }
    }

    private IEnumerator WaitForUpdateHold()
    {
        yield return new WaitForSeconds(isHoldDelay);
        isHold = true;
    }
    private IEnumerator WaitForSendNoteContext()
    {
        yield return new WaitForSeconds(combosDelay);
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