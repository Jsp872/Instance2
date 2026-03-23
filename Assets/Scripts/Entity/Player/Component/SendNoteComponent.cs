using UnityEngine.InputSystem;

// Enumération représentant les différentes notes musicales pouvant être envoyées.
public enum NoteID : byte
{
    DO = 0,
    RE = 1,
    MI = 2,
    FA = 3,
    NONE = 255,
}

/// <summary>
/// Composant responsable de l'envoi d'événements de note via l'EventBus.
/// </summary>
public class SendNoteComponent : PlayerComponent
{
    /// <summary>
    /// Initialise le composant avec le contrôleur du joueur.
    /// </summary>
    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        Log("[SendNoteComponent] Initialisé avec PlayerController: " + controller.name);
    }

    /// <summary>
    /// Gère l'entrée utilisateur pour envoyer une note via l'EventBus.
    /// </summary>
    public void HandleInput(InputAction.CallbackContext ctx, NoteID id)
    {
        if (!ctx.started) return;
        Log($"[SendNoteComponent] Note envoyée: {id}");
        EventBus.Publish(id);
    }
}