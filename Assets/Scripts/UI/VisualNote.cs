using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// VisualNote : synchronise l'affichage des rangées de notes (NoteRow) avec la séquence cible d'un obstacle Simon.
/// 
/// Utilisation :
/// 1. Ajouter le prefab VisualNote en tant qu'enfant de l'obstacle, au même niveau qu'un ObstaclePlayerDetector (frère dans la hiérarchie).
/// 2. Dans l'Inspector, assignez les NoteRow à piloter dans la liste "noteRows".
/// 3. (Optionnel) Réglez les couleurs dans "noteColorInRowOrder" pour chaque note.
/// 4. (Optionnel) Placez le RectTransform "layout" si vous souhaitez animer la fermeture.
/// 5. (Optionnel) Activez "debugLog" pour afficher les logs détaillés en Play Mode.
/// 6. N'oubliez pas de reset la position du VisualNote si besoin (remettre à zéro dans l'Inspector ou par script).
/// 
/// Prérequis :
/// - L'obstacle parent (ou l'un de ses enfants) doit porter un ObstaclePlayerDetector.
/// - Les NoteRow doivent être des enfants du VisualNote ou référencées dans la liste.
/// </summary>
public class VisualNote : MonoBehaviour
{
    // ─── Inspector ───────────────────────────────────────────────────────────

    [SerializeField] private List<Color> noteColorInRowOrder = new();
    [SerializeField] private RectTransform layout;

    [Tooltip("Rangées UI à initialiser avec le nombre de notes de la séquence cible.")] [SerializeField]
    private List<NoteRow> noteRows;

    private List<Note> track = new();

    // ─── Privé ───────────────────────────────────────────────────────────────

    private ObstaclePlayerDetector obstaclePlayerDetector;

    [Header("Debug")] [Tooltip("Active ou désactive les logs de debug pour ce VisualNote.")] [SerializeField]
    private bool debugLog = false;

    // ─── Lifecycle ───────────────────────────────────────────────────────────

    private void Awake()
    {
        // Remonte dans la hiérarchie pour trouver l'ObstaclePlayerDetector parent
        obstaclePlayerDetector = transform.parent.GetComponentInChildren<ObstaclePlayerDetector>();

        if (obstaclePlayerDetector == null)
        {
            Debug.LogError(
                $"[VisualNote] Aucun ObstaclePlayerDetector trouvé dans le parent de '{gameObject.name}'. Vérifier la hiérarchie.",
                this);
        }
        else
        {
            if (debugLog)
                Debug.Log(
                    $"[VisualNote] Awake → ObstaclePlayerDetector trouvé : '{obstaclePlayerDetector.gameObject.name}'.",
                    this);
        }

        if (noteRows == null || noteRows.Count == 0)
        {
            Debug.LogWarning($"[VisualNote] Aucune NoteRow assignée sur '{gameObject.name}'. Rien ne sera affiché.",
                this);
        }
        else
        {
            if (debugLog)
                Debug.Log($"[VisualNote] Awake → {noteRows.Count} NoteRow(s) assignée(s).", this);
        }
    }

    private void Start()
    {
        if (obstaclePlayerDetector == null) return;

        int count = obstaclePlayerDetector.sequenceCible.Count;
        if (debugLog)
            Debug.Log($"[VisualNote] Start → séquence cible de {count} note(s) détectée. Initialisation des NoteRow.",
                this);

        // Initialise chaque rangée avec le nombre exact de cases de la séquence
        foreach (NoteRow noteRow in noteRows)
        {
            if (noteRow == null)
            {
                if (debugLog)
                    Debug.LogWarning($"[VisualNote] Une NoteRow dans la liste est null — ignorée.", this);
                continue;
            }

            noteRow.InitializeNotes(count);
            noteRow.HideAll();
            if (debugLog)
                Debug.Log($"[VisualNote] Start → NoteRow '{noteRow.gameObject.name}' initialisée avec {count} note(s).",
                    this);
        }


        for (int i = 0; i < count; i++)
        {
            int rowIndex = (int)obstaclePlayerDetector.sequenceCible[i].note;
            Note note = noteRows[rowIndex].GetNote(i);
            track.Add(note);
            if (debugLog)
                Debug.Log($"[VisualNote] Start → Note {i} ajoutée à la track : {note.gameObject.name}", this);
            note.SetColor(noteColorInRowOrder[rowIndex]);
            note.Show();
        }

        track[0].Enable();
        if (track.Count > 1)
        {
            track[1].Enable(.75F);
        }

        obstaclePlayerDetector.nextNote += OnNewNote;
        obstaclePlayerDetector.badNote += OnBadNote;
        obstaclePlayerDetector.unlocked += OnSequenceUnlocked;
    }

    private void OnSequenceUnlocked()
    {
        foreach (NoteRow noteRow in noteRows)
        {
            noteRow.HideRow();
        }

        layout.DOSizeDelta(new Vector2(0, 0), 0.5f).SetEase(Ease.InQuad);
        if (debugLog)
            Debug.Log($"[VisualNote] Séquence déverrouillée → animation de fermeture déclenchée.", this);
    }

    private void OnBadNote()
    {
        foreach (Note note in track)
        {
            note.Reset();
        }

        track[0].Enable();
        if (track.Count > 1)
        {
            track[1].Enable(.75F);
        }

        if (debugLog)
            Debug.Log($"[VisualNote] OnBadNote() → toutes les notes réinitialisées, première note réactivée.", this);
    }

    private void OnNewNote()
    {
        if (obstaclePlayerDetector.indexCourant > 0 && track.Count > obstaclePlayerDetector.indexCourant)
        {
            track[obstaclePlayerDetector.indexCourant - 1].Disable();
            track[obstaclePlayerDetector.indexCourant].Enable();
            if (obstaclePlayerDetector.indexCourant + 1 < track.Count)
            {
                track[obstaclePlayerDetector.indexCourant + 1].Enable(.75f);
            }

            if (debugLog)
                Debug.Log(
                    $"[VisualNote] OnNewNote() → note {obstaclePlayerDetector.indexCourant} activée, précédente désactivée.",
                    this);
        }
    }
}