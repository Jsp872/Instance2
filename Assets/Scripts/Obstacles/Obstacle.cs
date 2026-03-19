using System;
using System.Collections.Generic;
using UnityEngine;

// Structures pour signaler l'entrée/sortie d'un obstacle dans le champ de vision.
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
    [Header("Debug")] [SerializeField] private bool debugLogs = false;

    [field: SerializeField] public List<NoteID> sequenceCible { get; private set; } = new();
    public int indexCourant { get; private set; }

    public event Action unlocked;
    public event Action<Obstacle> goodNote;
    public event Action badNote;
    private bool unlock;
    private bool definitivelyLocked;

    /// <summary>
    /// Appelé quand l'obstacle devient visible dans la scène.
    /// </summary>
    private void OnBecameVisible()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
        if (unlock || definitivelyLocked)
        {
            if (debugLogs)
                Debug.Log($"[Obstacle] OnBecameVisible: déjà unlock ou définitivement verrouillé.", this);
            return;
        }
        EventBus.Publish(new ObstacleEnteredView { obstacle = this });
        EventBus.Subscribe<NoteID>(OnNoteReceived);
        if (debugLogs)
            Debug.Log($"[Obstacle] OnBecameVisible: EventBus.Subscribe<NoteID>", this);
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
    /// Appelé quand l'obstacle devient invisible dans la scène.
    /// </summary>
    private void OnBecameInvisible()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
        EventBus.Publish(new ObstacleExitedView { obstacle = this });
        EventBus.Unsubscribe<NoteID>(OnNoteReceived);
        if (debugLogs)
            Debug.Log($"[Obstacle] OnBecameInvisible: EventBus.Unsubscribe<NoteID>", this);
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
        }
        else
        {
            badNote?.Invoke();
            definitivelyLocked = true;
            if (debugLogs)
                Debug.Log($"[Obstacle] Mauvaise note reçue: {receivedNote}, obstacle verrouillé.", this);
        }
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
    }
}