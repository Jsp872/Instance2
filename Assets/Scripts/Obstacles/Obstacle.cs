using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public struct ObstacleEnteredView
{
    public Obstacle obstacle;
    public float noteIntervalSpeed;
}

public struct ObstacleExitedView
{
    public Obstacle obstacle;
}

/// <summary>
/// Classe abstraite représentant un obstacle interactif.
/// Gère la séquence de notes, l'état de verrouillage et la logique d'événement.
/// </summary>
public abstract class Obstacle : MonoBehaviour
{
    [Header("Debug")][SerializeField] protected bool debugLogs = false;

    [Header("Note Speed")][SerializeField] protected float noteIntervalSpeed = .1f;

    private Camera _camera;

    [field: SerializeField] public List<NoteID> sequenceCible { get; private set; } = new();
    public int indexCourant { get; private set; }

    public event Action unlocked;
    public event Action<Obstacle> goodNote;
    public event Action badNote;

    private bool unlock;
    private bool definitivelyLocked;
    private bool _isSubscribed = false;
    private Renderer _renderer;
    private Plane[] _frustumPlanes;

    private void Awake()
    {
        _camera = Camera.main;
        _renderer = GetComponentInChildren<Renderer>();

        if (_camera == null)
            Debug.LogWarning($"[Obstacle] Aucune caméra assignée sur {gameObject.name}.", this);
        Init();
    }

    private void Update()
    {
        if (_camera == null || _renderer == null) return;
        if (unlock || definitivelyLocked) return;

        _frustumPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);
        bool isVisibleNow = GeometryUtility.TestPlanesAABB(_frustumPlanes, _renderer.bounds);
        Log($"[Obstacle] isVisibleNow: {isVisibleNow}");
        if (isVisibleNow && !_isSubscribed)
            HandleEnteredView();
        else if (!isVisibleNow && _isSubscribed)
            HandleExitedView();
    }

    private void HandleEnteredView()
    {
        _isSubscribed = true;
        EventBus.Publish(new ObstacleEnteredView { obstacle = this, noteIntervalSpeed = noteIntervalSpeed });
        EventBus.Subscribe<NoteID>(OnNoteReceived);

        Log($"[Obstacle] HandleEnteredView: EventBus.Subscribe<NoteID>");
    }

    private void HandleExitedView()
    {
        _isSubscribed = false;
        EventBus.Publish(new ObstacleExitedView { obstacle = this });
        EventBus.Unsubscribe<NoteID>(OnNoteReceived);

        Log($"[Obstacle] HandleExitedView: EventBus.Unsubscribe<NoteID>");
    }

    /// <summary>
    /// Callback pour la réception d'une note.
    /// </summary>
    private void OnNoteReceived(NoteID id)
    {
        Log($"[Obstacle] OnNoteReceived: {id}");
        CheckNote(id);
    }

    /// <summary>
    /// Vérifie la note reçue et met à jour l'état de l'obstacle.
    /// </summary>
    public void CheckNote(NoteID receivedNote)
    {
        if (unlock || definitivelyLocked)
        {
            Log($"[Obstacle] CheckNote: déjà unlock ou définitivement verrouillé.");
            return;
        }

        if (sequenceCible == null || sequenceCible.Count == 0)
        {
            Debug.LogWarning($"[{nameof(Obstacle)}] Aucune séquence configurée sur {gameObject.name}.", this);
            return;
        }

        if (receivedNote == sequenceCible[indexCourant])
        {
            indexCourant++;
            goodNote?.Invoke(this);

            Log($"[Obstacle] Bonne note reçue: {receivedNote}, indexCourant={indexCourant}");

            if (indexCourant >= sequenceCible.Count)
                Unlock();
            else
            {
                UnLockingBehaviour();
            }
        }
        else
        {
            badNote?.Invoke();
            definitivelyLocked = true;
            Log($"[Obstacle] Mauvaise note reçue: {receivedNote}, obstacle verrouillé.");
            LockedBehaviour();
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<NoteID>(OnNoteReceived);
    }

    /// <summary>
    /// Réinitialise la séquence de l'obstacle.
    /// </summary>
    public void ResetSequence()
    {
        indexCourant = 0;
        OnSequenceReset();
        Log($"[Obstacle] ResetSequence: indexCourant réinitialisé.");
    }

    /// <summary>
    /// Méthode virtuelle pour la logique de réinitialisation personnalisée.
    /// </summary>
    protected virtual void OnSequenceReset()
    {
    }

    /// <summary>
    /// Déverrouille l'obstacle.
    /// </summary>
    private void Unlock()
    {
        unlock = true;
        unlocked?.Invoke();
        Log($"[Obstacle] Unlock: obstacle déverrouillé.");
        UnlockedBehaviour();
    }

    public void Log(string message) { if (debugLogs) Debug.Log(message, this); }

    //overridable functions
    protected virtual void Init()
    {

    }

    protected virtual void UnLockingBehaviour()
    {

    }

    protected virtual void UnlockedBehaviour()
    {

    }

    protected virtual void LockedBehaviour()
    {

    }
}
