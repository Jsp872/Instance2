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
    private bool isNearToObstacle = false;
    /// <summary>
    /// Initialise le composant avec le contrôleur du joueur.
    /// </summary>
    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);

        EventBus.Subscribe<ObstacleEnteredView>(OnObstacleEnteredView);
        EventBus.Subscribe<ObstacleExitedView>(OnObstacleExitedView);
        
        Log("[SendNoteComponent] Initialisé avec PlayerController: " + controller.name);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<ObstacleEnteredView>(OnObstacleEnteredView);
        EventBus.Unsubscribe<ObstacleExitedView>(OnObstacleExitedView);
    }

    /// <summary>
    /// Gère l'entrée utilisateur pour envoyer une note via l'EventBus.
    /// </summary>
    /// 
    private void OnObstacleEnteredView(ObstacleEnteredView callback) => isNearToObstacle = true;
    private void OnObstacleExitedView(ObstacleExitedView callback) => isNearToObstacle = false;

    public void HandleInput(InputAction.CallbackContext ctx, NoteID id)
    {
        if (!ctx.started) return;
        Log($"[SendNoteComponent] Note envoyée: {id}");
        EventBus.Publish(id);

        if (!isNearToObstacle)
        {
            EventBus.Publish(new OnSendNoteSound(id));
        }
    }
}