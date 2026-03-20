using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Gère la liste des obstacles actifs pour le joueur.
/// </summary>
public class PlayerObstacleHandler : PlayerComponent
{
    [Header("Debug")] [SerializeField] private bool debugLogs = false;
    private readonly List<Obstacle> activeObstacles = new();

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        if (debugLogs)
            Debug.Log("[PlayerObstacleHandler] Initialize: PlayerController initialisé.", this);
    }

    public override void UpdateComponent(ref Vector3 velocity, float dT) { }

    private void OnEnable()
    {
        EventBus.Subscribe<ObstacleEnteredView>(OnObstacleEnteredView);
        EventBus.Subscribe<ObstacleExitedView>(OnObstacleExitedView);
        if (debugLogs)
            Debug.Log("[PlayerObstacleHandler] OnEnable: Subscribed to events.", this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ObstacleEnteredView>(OnObstacleEnteredView);
        EventBus.Unsubscribe<ObstacleExitedView>(OnObstacleExitedView);
        if (debugLogs)
            Debug.Log("[PlayerObstacleHandler] OnDisable: Unsubscribed from events.", this);
    }

    private void OnObstacleEnteredView(ObstacleEnteredView e)
    {
        activeObstacles.Add(e.obstacle);
        e.obstacle.ResetSequence();
        if (debugLogs)
            Debug.Log($"[PlayerObstacleHandler] OnObstacleEnteredView: obstacle ajouté {e.obstacle.name}", this);
    }

    private void OnObstacleExitedView(ObstacleExitedView e)
    {
        activeObstacles.Remove(e.obstacle);
        e.obstacle.ResetSequence();
        if (debugLogs)
            Debug.Log($"[PlayerObstacleHandler] OnObstacleExitedView: obstacle retiré {e.obstacle.name}", this);
    }
}