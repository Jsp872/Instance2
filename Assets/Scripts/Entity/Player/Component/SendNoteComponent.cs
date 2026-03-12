using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SendNoteComponent : PlayerComponent
{
    // je vais refacto tout ça pour l'instant au moins ça marche
    private NoteID currentNote;
    public void OnSendDO(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            currentNote = NoteID.DO;
            OnActionStarted();
        }
    }
    public void OnSendRE(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            currentNote = NoteID.RE;
            OnActionStarted();
        }
    }
    public void OnSendMI(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            currentNote = NoteID.MI;
            OnActionStarted();
        }
    }
    public void OnSendFA(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            currentNote = NoteID.FA;
            OnActionStarted();
        }
    }


    public override void OnActionStarted()
    {
        base.OnActionStarted();
        EventBus.Publish(new SendNoteCallback(currentNote, 1, 0.0f));
    }
    public override void OnActionCanceled()
    {
        base.OnActionCanceled();
    }
}

public enum NoteID : byte
{
    DO = 0,
    RE = 1,
    MI = 2,
    FA = 3,
}