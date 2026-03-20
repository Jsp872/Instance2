using System;
using System.Collections.Generic;
using UnityEngine;

public struct ObstacleEnteredView
{
    public Obstacle obstacle;
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
    [Header("Debug")] [SerializeField] protected bool debugLogs = false;

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
        Debug.Log($"[Obstacle] isVisibleNow: {isVisibleNow}", this);

        if (isVisibleNow && !_isSubscribed)
            HandleEnteredView();
        else if (!isVisibleNow && _isSubscribed)
            HandleExitedView();
    }

    private void HandleEnteredView()
    {
        _isSubscribed = true;
        EventBus.Publish(new ObstacleEnteredView { obstacle = this });
        EventBus.Subscribe<NoteID>(OnNoteReceived);

        if (debugLogs)
            Debug.Log($"[Obstacle] HandleEnteredView: EventBus.Subscribe<NoteID>", this);
    }

    private void HandleExitedView()
    {
        _isSubscribed = false;
        EventBus.Publish(new ObstacleExitedView { obstacle = this });
        EventBus.Unsubscribe<NoteID>(OnNoteReceived);

        if (debugLogs)
            Debug.Log($"[Obstacle] HandleExitedView: EventBus.Unsubscribe<NoteID>", this);
    }

    /// <summary>
    /// Callback pour la réception d'une note.
    /// </summary>
    private void OnNoteReceived(NoteID id)
    {
        if (debugLogs)
            Debug.Log($"[Obstacle] OnNoteReceived: {id}", this);
        CheckNote(id);
    }

    /// <summary>
    /// Vérifie la note reçue et met à jour l'état de l'obstacle.
    /// </summary>
    public void CheckNote(NoteID receivedNote)
    {
        if (unlock || definitivelyLocked)
        {
            if (debugLogs)
                Debug.Log($"[Obstacle] CheckNote: déjà unlock ou définitivement verrouillé.", this);
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
            if (debugLogs)
                Debug.Log($"[Obstacle] Bonne note reçue: {receivedNote}, indexCourant={indexCourant}", this);
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
            if (debugLogs)
                Debug.Log($"[Obstacle] Mauvaise note reçue: {receivedNote}, obstacle verrouillé.", this);
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
        if (debugLogs)
            Debug.Log($"[Obstacle] ResetSequence: indexCourant réinitialisé.", this);
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
        if (debugLogs)
            Debug.Log($"[Obstacle] Unlock: obstacle déverrouillé.", this);
        UnlockedBehaviour();
    }
    
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
