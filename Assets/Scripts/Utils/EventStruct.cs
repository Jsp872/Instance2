
using UnityEngine;

public struct LooseMaxSpeedCallback { }
public struct MaxSpeedReachCallback { }
public struct OnHitObstacleCallback { public Transform obstacle; }

public struct OnJumpStarted { }
public struct OnApexReached { }
public struct OnFallStarted { }
public struct OnJumpFinished { }
public struct OnSendNoteSound
{
    public NoteID id;
    public OnSendNoteSound(NoteID id)
    {
        this.id = id;
    }
}
