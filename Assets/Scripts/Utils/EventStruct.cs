public struct SendNoteCallback
{
    public NoteID note;
    public int noteSendCount;
    public float holdDuration;
    public SendNoteCallback(NoteID note, int noteSendCount, float holdDuration)
    {
        this.note = note;
        this.noteSendCount = noteSendCount;
        this.holdDuration = holdDuration;
    }
}