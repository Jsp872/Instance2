using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePlayerDetector : MonoBehaviour
{
    [SerializeField] private List<InputEnum> sequenceCible;
    private int indexCourant = 0;
    public event Action unlocked;

    // ─── Zone d'interaction ──────────────────────────────────────────────────

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Player>(out _)) return;

        EventBus.Subscribe<NoteEvent>(CheckInteraction);
        PlaySequence();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<Player>(out _)) return;

        EventBus.Unsubscribe<NoteEvent>(CheckInteraction);
    }

    // ─── Séquence ────────────────────────────────────────────────────────────

    private void PlaySequence()
    {
        // TODO: Déclencher la lecture visuelle/sonore de sequenceCible
    }

    // ─── Callback Event Bus ──────────────────────────────────────────────────

    private void CheckInteraction(NoteEvent evt)
    {
        if (evt.Note != sequenceCible[indexCourant])
        {
            // Mauvaise note : reset et relecture de la séquence
            indexCourant = 0;
            PlaySequence();
            Debug.Log("Mauvaise note  : " + evt.Note + ", reset de la séquence");
            return;
        }

        // Bonne note : avancer dans la séquence
        indexCourant++;
        Debug.Log("Bonne note : " + evt.Note);

        if (indexCourant >= sequenceCible.Count)
        {
            Unlock();
        }
    }

    // ─── Déverrouillage ──────────────────────────────────────────────────────

    private void Unlock()
    {
        EventBus.Unsubscribe<NoteEvent>(CheckInteraction);
        unlocked?.Invoke();
        // TODO: Logique de franchissement (désactivation collider, animation, VFX, etc.)
    }
}
