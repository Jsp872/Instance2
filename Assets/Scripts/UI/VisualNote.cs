using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Gère l'affichage des notes visuelles dans l'UI.
/// </summary>
public class VisualNote : MonoBehaviour
{
    [SerializeField] private List<Color> noteColorInRowOrder = new();
    [SerializeField] private RectTransform layout;
    [SerializeField] private Note notePrefab;
    [SerializeField] private AutoMoveComponent playerMovement;

    [Tooltip("Rangées UI à initialiser avec le nombre de notes de la séquence cible.")] [SerializeField]
    private List<RectTransform> noteRows;

    private List<Note> notes = new();

    [Header("Debug")] [SerializeField] private bool debugLogs = false;
    [SerializeField] private int obstacleCountBeforeHiding;
    private int obstacleCount;

    private void Awake()
    {
        if (noteRows == null || noteRows.Count == 0)
            Debug.LogWarning($"[VisualNote] Aucune NoteRow assignée sur '{gameObject.name}'.", this);
    }

    private void OnEnable() => EventBus.Subscribe<ObstacleEnteredView>(OnNewObstacle);
    private void OnDisable() => EventBus.Unsubscribe<ObstacleEnteredView>(OnNewObstacle);

    /// <summary>
    /// Callback lors de l'entrée d'un nouvel obstacle.
    /// </summary>
    private void OnNewObstacle(ObstacleEnteredView e)
    {
        Obstacle obstacle = e.obstacle;
        if (obstacle == null) return;
        if (playerMovement == null)
        {
            Debug.LogWarning($"[VisualNote] playerMovement est null ou détruit.", this);
            return;
        }

        obstacle.goodNote += OnGoodNote;
        obstacle.badNote += OnBadNote;
        obstacleCount++;
        if (obstacleCount >= obstacleCountBeforeHiding + 1)
        {
            if (debugLogs)
                Debug.Log($"[VisualNote] Nombre d'obstacles atteint ({obstacleCount}), désactivation du composant.", this);
            GetComponent<CanvasGroup>().Toggle(false);
        }

        StartCoroutine(ProcessNotes(e));
    }

    /// <summary>
    /// Callback lors d'une mauvaise note.
    /// </summary>
    private void OnBadNote()
    {
        if (debugLogs)
            Debug.Log($"[VisualNote] Note ratée !", this);
        foreach (Note note in notes)
        {
            DOTween.Kill(note.GetComponent<RectTransform>());
            note.SetColor(Color.black);
        }

        notes.Clear();
    }

    /// <summary>
    /// Callback lors d'une bonne note.
    /// </summary>
    private void OnGoodNote(Obstacle obstacle)
    {
        if (debugLogs)
            Debug.Log($"[VisualNote] Note réussie !", this);
        notes[0].Disable();
        notes.RemoveAt(0);
    }

    /// <summary>
    /// Coroutine pour traiter l'affichage des notes.
    /// </summary>
    private IEnumerator ProcessNotes(ObstacleEnteredView obstacleCtx)
    {
        Obstacle obstacle = obstacleCtx.obstacle;

        for (var i = 0; i < obstacle.sequenceCible.Count; i++)
        {
            NoteID note = obstacle.sequenceCible[i];
            RectTransform row = noteRows[(int)note];
            Note newNote = Instantiate(notePrefab, row);
            float distance = (obstacle.transform.position - playerMovement.transform.position).magnitude;
            float speed = layout.rect.width / distance * playerMovement.currentSpeed;
            
            print($"[TEST]__Send Sound Note : {note}");
           

            if (debugLogs)
                Debug.Log($"[VisualNote] Nouvelle note pour '{obstacle.gameObject.name}' → vitesse calculée : {speed:F2}", this);

            newNote.StartMove(speed, layout.rect.width);
            newNote.SetColor(noteColorInRowOrder[(int)note]);
            notes.Add(newNote);
            if (i < obstacle.sequenceCible.Count - 1)
            {
                yield return new WaitForSeconds(obstacleCtx.noteIntervalSpeed);
            }

        }

        yield return null;
    }
}