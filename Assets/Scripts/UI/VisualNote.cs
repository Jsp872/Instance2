using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère l'affichage des notes visuelles dans l'UI.
/// </summary>
public class VisualNote : MonoBehaviour
{
    [SerializeField] private List<Sprite> noteColorInRowOrder = new();
    [SerializeField] private RectTransform layout;
    [SerializeField] private Note notePrefab;
    [SerializeField] private AutoMoveComponent playerMovement;

    [Tooltip("Rangées UI à initialiser avec le nombre de notes de la séquence cible.")]
    [SerializeField]
    private List<RectTransform> noteRows;

    private List<Note> notes = new();

    [Header("Debug")][SerializeField] private bool debugLogs = false;
    [SerializeField] private int obstacleCountBeforeHiding;
    private int obstacleCount;

    private bool cheatHelperEnable = false;

    private void Awake()
    {
        if (noteRows == null || noteRows.Count == 0)
            Debug.LogWarning($"[VisualNote] Aucune NoteRow assignée sur '{gameObject.name}'.", this);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<ObstacleEnteredView>(OnNewObstacle);
        EventBus.Subscribe<ActiveVisibleNoteHelper>(EnableCheatHelperPanel);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<ObstacleEnteredView>(OnNewObstacle);
        EventBus.Unsubscribe<ActiveVisibleNoteHelper>(EnableCheatHelperPanel);
    }


    private void EnableCheatHelperPanel(ActiveVisibleNoteHelper callback)
    {
        cheatHelperEnable = callback.isActive;

        if (obstacleCount >= obstacleCountBeforeHiding + 1)
        {
            GetComponent<CanvasGroup>().Toggle(cheatHelperEnable);
        }
    }

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
        if (!cheatHelperEnable && obstacleCount >= obstacleCountBeforeHiding + 1)
        {
            print(cheatHelperEnable);
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
        EventBus.Publish<OnMissSound>(new OnMissSound());
        print("_________________CALL OnMissSound______________");

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
    private void OnGoodNote(OnGoodNote callback)
    {
        if (debugLogs)
            Debug.Log($"[VisualNote] Note réussie !", this);

        EventBus.Publish(new OnSendNoteSound(callback.note));

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

            EventBus.Publish(new OnSendNoteSound(note));

            if (debugLogs)
                Debug.Log($"[VisualNote] Nouvelle note pour '{obstacle.gameObject.name}' -> vitesse calculée : {speed:F2}", this);

            newNote.StartMove(speed, layout.rect.width);
            newNote.SetSprite(noteColorInRowOrder[(int)note]);
            notes.Add(newNote);
            if (i < obstacle.sequenceCible.Count - 1)
            {
                yield return new WaitForSeconds(obstacleCtx.noteIntervalSpeed);
            }
        }

        yield return null;
    }
}