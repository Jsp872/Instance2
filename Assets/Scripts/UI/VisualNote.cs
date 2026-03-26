using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualNote : MonoBehaviour
{
    [SerializeField] private List<Sprite> noteColorInRowOrder = new();
    [SerializeField] private RectTransform layout;
    [SerializeField] private Note notePrefab;
    [SerializeField] private AutoMoveComponent playerMovement;

    [SerializeField] private List<RectTransform> noteRows;

    private List<Note> notes = new();

    [Header("Debug")]
    [SerializeField] private bool debugLogs = false;

    [SerializeField] private int obstacleCountBeforeHiding;
    private int obstacleCount;

    private bool cheatHelperEnable = false;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (noteRows == null || noteRows.Count == 0)
            Debug.LogWarning($"[VisualNote] No NoteRows assigned on '{gameObject.name}'.", this);
    }

    private void Start()
    {
        cheatHelperEnable = PlayerBlackboard.Instance.enableVisualHelper;
        UpdateVisibility();
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

    private void UpdateVisibility()
    {
        if (obstacleCount < obstacleCountBeforeHiding)
            return;

        canvasGroup.Toggle(cheatHelperEnable);
    }

    private void EnableCheatHelperPanel(ActiveVisibleNoteHelper callback)
    {
        cheatHelperEnable = callback.isActive;
        UpdateVisibility();
    }

    private void OnNewObstacle(ObstacleEnteredView e)
    {
        Obstacle obstacle = e.obstacle;
        if (obstacle == null) return;

        if (playerMovement == null)
        {
            Debug.LogWarning($"[VisualNote] playerMovement is null.", this);
            return;
        }

        obstacle.goodNote += OnGoodNote;
        obstacle.badNote += OnBadNote;

        obstacleCount++;
        UpdateVisibility();

        StartCoroutine(ProcessNotes(e));
    }

    private void OnBadNote()
    {
        if (debugLogs)
            Debug.Log("[VisualNote] Bad note");

        EventBus.Publish(new OnMissSound());

        foreach (Note note in notes)
        {
            DOTween.Kill(note.GetComponent<RectTransform>());
            note.SetColor(Color.black);
        }

        notes.Clear();
    }

    private void OnGoodNote(OnGoodNote callback)
    {
        if (debugLogs)
            Debug.Log("[VisualNote] Good note");

        EventBus.Publish(new OnSendNoteSound(callback.note));

        notes[0].Disable();
        notes.RemoveAt(0);
    }

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
                Debug.Log($"[VisualNote] Speed: {speed:F2}");

            newNote.StartMove(speed, layout.rect.width);
            newNote.SetSprite(noteColorInRowOrder[(int)note]);

            notes.Add(newNote);

            if (i < obstacle.sequenceCible.Count - 1)
                yield return new WaitForSeconds(obstacleCtx.noteIntervalSpeed);
        }
    }
}