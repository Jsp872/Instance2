/// <summary>
/// Représente les notes jouables par le joueur dans les obstacles Simon.
/// </summary>
public enum InputEnum
{
    Note1,
    Note2,
    Note3,
    Note4,
}

/// <summary>
/// Payload de l'Event Bus transmis à chaque fois que le joueur joue une note.
/// Publier via : EventBus.Publish(new NoteEvent { Note = InputEnum.Note1 });
/// </summary>
[System.Serializable]
public struct NoteEvent
{
    public InputEnum Note;
}

[System.Serializable]
public struct JumpEvent
{
}
