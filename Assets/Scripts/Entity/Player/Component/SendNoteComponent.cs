using UnityEngine;
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
    // Permet d'activer/désactiver les logs de debug dans ce composant depuis l'inspecteur.
    [SerializeField] private bool debugLogs = false;

    /// <summary>
    /// Initialise le composant avec le contrôleur du joueur.
    /// </summary>
    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        if (debugLogs)
            Debug.Log("[SendNoteComponent] Initialisé avec PlayerController: " + controller.name);
    }

    /// <summary>
    /// Mise à jour du composant à chaque frame (non utilisé ici).
    /// </summary>
    public override void UpdateComponent(ref Vector3 velocity, float dT) { }

    /// <summary>
    /// Gère l'entrée utilisateur pour envoyer une note via l'EventBus.
    /// </summary>
    public void HandleInput(InputAction.CallbackContext ctx, NoteID id)
    {
        if (!ctx.started) return;

        if (debugLogs)
            Debug.Log($"[SendNoteComponent] Note envoyée: {id}");

        EventBus.Publish(id);
    }
}