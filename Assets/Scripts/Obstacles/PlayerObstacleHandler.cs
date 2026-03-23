using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Gère la liste des obstacles actifs pour le joueur.
/// </summary>
public class PlayerObstacleHandler : PlayerComponent
{
    private readonly List<Obstacle> activeObstacles = new();

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        Log("[PlayerObstacleHandler] Initialize: PlayerController initialisé.", this);
    }

    public override void UpdateComponent(ref Vector3 velocity, float dT) { }

    private void OnEnable()
    {
        EventBus.Subscribe<ObstacleEnteredView>(OnObstacleEnteredView);
        EventBus.Subscribe<ObstacleExitedView>(OnObstacleExitedView);
        Log("[PlayerObstacleHandler] OnEnable: Subscribed to events.", this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ObstacleEnteredView>(OnObstacleEnteredView);
        EventBus.Unsubscribe<ObstacleExitedView>(OnObstacleExitedView);
        Log("[PlayerObstacleHandler] OnDisable: Unsubscribed from events.", this);
    }

    private void OnObstacleEnteredView(ObstacleEnteredView e)
    {
        activeObstacles.Add(e.obstacle);
        e.obstacle.ResetSequence();
        Log($"[PlayerObstacleHandler] OnObstacleEnteredView: obstacle ajouté {e.obstacle.name}", this);
    }

    private void OnObstacleExitedView(ObstacleExitedView e)
    {
        activeObstacles.Remove(e.obstacle);
        e.obstacle.ResetSequence();
        Log($"[PlayerObstacleHandler] OnObstacleExitedView: obstacle retiré {e.obstacle.name}", this);
    }
}