[System.Serializable]
public struct NoteContext
{
    public NoteID note;
    public float holdDuration;
    public int noteSendCount;
    public NoteContext(NoteID note, float holdDuration, int noteSendCount = 1)
    {
        this.note = note;
        this.holdDuration = holdDuration;
        this.noteSendCount = noteSendCount;
    }
}