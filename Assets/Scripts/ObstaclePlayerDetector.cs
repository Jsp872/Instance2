using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePlayerDetector : MonoBehaviour
{
    [field: SerializeField] public List<NoteContext> sequenceCible { get; private set; }  = new();
    public int indexCourant { get; private set; }
    private bool isListening;
    public event Action unlocked;
    public event Action nextNote;
    public event Action badNote;

    // ─── Zone d'interaction ──────────────────────────────────────────────────

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Player>(out _)) return;

        indexCourant = 0;
        StartListening();
        PlaySequence();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<Player>(out _)) return;

        StopListening();
        indexCourant = 0;
    }

    private void OnDisable()
    {
        StopListening();
        indexCourant = 0;
    }

    // ─── Séquence ────────────────────────────────────────────────────────────

    private void PlaySequence()
    {
        // TODO: Déclencher la lecture visuelle/sonore de sequenceCiblee
    }

    // ─── Callback Event Bus ──────────────────────────────────────────────────

    private void CheckInteraction(NoteContext evt)
    {
        if (sequenceCible == null || sequenceCible.Count == 0)
        {
            Debug.LogWarning($"[{nameof(ObstaclePlayerDetector)}] Aucune séquence cible configurée sur {gameObject.name}.");
            return;
        }

        NoteContext contexteAttendu = sequenceCible[indexCourant];

        if (!MatchesExpectedContext(contexteAttendu, evt))
        {
            // Mauvaise note : reset et relecture de la séquence
            indexCourant = 0;
            badNote?.Invoke();
            PlaySequence();
            Debug.Log($"Mauvaise note : reçu {FormatContext(evt)}, attendu {FormatContext(contexteAttendu)}. Reset de la séquence.");
            return;
        }

        // Bonne note : avancer dans la séquence
        indexCourant++;
        nextNote?.Invoke();
        Debug.Log("Bonne note : " + evt.note);

        if (indexCourant >= sequenceCible.Count)
        {
            Unlock();
        }
    }

    // ─── Déverrouillage ──────────────────────────────────────────────────────

    private void Unlock()
    {
        StopListening();
        unlocked?.Invoke();
    }

    private void StartListening()
    {
        if (isListening)
            return;

        EventBus.Subscribe<NoteContext>(CheckInteraction);
        isListening = true;
    }

    private void StopListening()
    {
        if (!isListening)
            return;

        EventBus.Unsubscribe<NoteContext>(CheckInteraction);
        isListening = false;
    }

    private static bool MatchesExpectedContext(NoteContext expected, NoteContext received)
    {
        if (expected.note != received.note)
            return false;

        if (expected.noteSendCount > 0 && expected.noteSendCount != received.noteSendCount)
            return false;

        if (expected.holdDuration > 0f && received.holdDuration + 0.01f < expected.holdDuration)
            return false;

        return true;
    }

    private static string FormatContext(NoteContext context)
    {
        return $"{context.note} (count: {context.noteSendCount}, hold: {context.holdDuration:0.##})";
    }
}
