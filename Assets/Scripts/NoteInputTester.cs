using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Système de test des 4 notes de l'EventBus.
/// Chaque action Input déclenche un NoteEvent publié sur l'EventBus.
///
/// Setup :
///   • Glisser les 4 actions depuis InputSystem_Actions dans les champs Inspector.
///   • Les abonnés à NoteEvent recevront les events normalement.
/// </summary>
public class NoteInputTester : MonoBehaviour
{
    // ─── Input ────────────────────────────────────────────────────────────────

    [Header("Actions Input")]
    [SerializeField] private InputActionReference note1Action;
    [SerializeField] private InputActionReference note2Action;
    [SerializeField] private InputActionReference note3Action;
    [SerializeField] private InputActionReference note4Action;

    // ─── Lifecycle ────────────────────────────────────────────────────────────

    private void OnEnable()
    {
        if (note1Action != null) { note1Action.action.Enable(); note1Action.action.performed += OnNote1Performed; }
        if (note2Action != null) { note2Action.action.Enable(); note2Action.action.performed += OnNote2Performed; }
        if (note3Action != null) { note3Action.action.Enable(); note3Action.action.performed += OnNote3Performed; }
        if (note4Action != null) { note4Action.action.Enable(); note4Action.action.performed += OnNote4Performed; }
    }

    private void OnDisable()
    {
        if (note1Action != null) { note1Action.action.performed -= OnNote1Performed; note1Action.action.Disable(); }
        if (note2Action != null) { note2Action.action.performed -= OnNote2Performed; note2Action.action.Disable(); }
        if (note3Action != null) { note3Action.action.performed -= OnNote3Performed; note3Action.action.Disable(); }
        if (note4Action != null) { note4Action.action.performed -= OnNote4Performed; note4Action.action.Disable(); }
    }

    // ─── Handlers ─────────────────────────────────────────────────────────────

    private void OnNote1Performed(InputAction.CallbackContext ctx) => Publish(InputEnum.Note1);
    private void OnNote2Performed(InputAction.CallbackContext ctx) => Publish(InputEnum.Note2);
    private void OnNote3Performed(InputAction.CallbackContext ctx) => Publish(InputEnum.Note3);
    private void OnNote4Performed(InputAction.CallbackContext ctx) => Publish(InputEnum.Note4);

    // ─── EventBus ────────────────────────────────────────────────────────────

    private void Publish(InputEnum note)
    {
        EventBus.Publish(new NoteEvent { Note = note });
    }
}
