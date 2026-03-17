using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gère une rangée de notes UI pour représenter visuellement une séquence Simon.
/// Spawn les <see cref="Note"/> dynamiquement à partir d'un préfab.
/// Appeler <see cref="InitializeNotes"/> pour (re)créer la rangée avec le bon nombre d'éléments.
/// </summary>
public class NoteRow : MonoBehaviour
{
    // ─── Inspector ───────────────────────────────────────────────────────────

    [Tooltip("Préfab Note à instancier pour chaque élément de la séquence.")]
    [SerializeField] private Note notePrefab;

    [SerializeField] private GameObject label;

    [Header("Debug")]
    [Tooltip("Active ou désactive les logs de debug pour cette rangée.")]
    [SerializeField] private bool debugLog = false;

    // ─── Privé ───────────────────────────────────────────────────────────────

    private List<Note> notes = new();
    private RectTransform rectTransform;

    // ─── Lifecycle ───────────────────────────────────────────────────────────

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (notePrefab == null)
        {
            Debug.LogError($"[NoteRow] 'notePrefab' non assigné sur {gameObject.name}. Assigner un préfab Note dans l'Inspector.", this);
        }
        else
        {
            if (debugLog)
                Debug.Log($"[NoteRow] Awake → RectTransform et préfab prêts sur '{gameObject.name}'.", this);
        }
    }

    // ─── API publique ─────────────────────────────────────────────────────────

    /// <summary>
    /// (Re)crée <paramref name="count"/> notes dans la rangée.
    /// Détruit les notes précédentes avant d'en créer de nouvelles.
    /// </summary>
    /// <param name="count">Nombre de cases à afficher.</param>
    public void InitializeNotes(int count)
    {
        if (debugLog)
            Debug.Log($"[NoteRow] InitializeNotes({count}) sur '{gameObject.name}' — suppression de {notes.Count} note(s) existante(s).", this);

        // Nettoyage des notes existantes
        foreach (Note note in notes)
        {
            Destroy(note.gameObject);
        }
        notes.Clear();

        // Création des nouvelles notes
        for (int i = 0; i < count; i++)
        {
            Note created = Instantiate(notePrefab, rectTransform);
            created.gameObject.name = $"Note_{i}";
            notes.Add(created);
            if (debugLog)
                Debug.Log($"[NoteRow] Note_{i} instanciée sous '{gameObject.name}'.", this);
        }

        // Force Unity Layout à recalculer la taille immédiatement
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        if (debugLog)
            Debug.Log($"[NoteRow] InitializeNotes terminé → {notes.Count} note(s) active(s) dans '{gameObject.name}'.", this);
    }

    // ─── Accès aux notes individuelles ───────────────────────────────────────

    /// <summary>Retourne la note à l'index donné, ou null si hors limites.</summary>
    public Note GetNote(int index)
    {
        if (index < 0 || index >= notes.Count)
        {
            Debug.LogWarning($"[NoteRow] GetNote({index}) hors limites (count={notes.Count}) sur '{gameObject.name}'.", this);
            return null;
        }

        return notes[index];
    }

    /// <summary>Nombre de notes actuellement présentes dans la rangée.</summary>
    public int Count => notes.Count;

    public void HideAll()
    {
        foreach (Note note in notes)
        {
            note.Hide();
        }
        if (debugLog)
            Debug.Log($"[NoteRow] HideAll() → toutes les notes masquées sur '{gameObject.name}'.", this);
    }

    public void HideRow()
    {
        foreach (Note note in notes)
        {
            note.Hide();
        }
        label.SetActive(false);
        if (debugLog)
            Debug.Log($"[NoteRow] HideRow() → notes et label masqués sur '{gameObject.name}'.", this);
    }
}