using TMPro;
using UnityEngine;

public class CheatMenu : UI_Basic_Functions
{
    [SerializeField] private GameObject cheatPanel;
    [SerializeField] private TextMeshProUGUI enableTextMesh1;
    [SerializeField] private TextMeshProUGUI enableTextMesh2;

    private bool enableHelper;

    private void Start()
    {
        enableHelper = PlayerBlackboard.Instance.enableVisualHelper;
        ApplyVisualState();
    }

    public void OpenCheatPanel(bool value)
    {
        cheatPanel.SetActive(value);
    }

    public void ActiveVisibleNoteHelper()
    {
        enableHelper = !enableHelper;

        PlayerBlackboard.Instance.enableVisualHelper = enableHelper;

        ApplyVisualState();
    }

    private void ApplyVisualState()
    {
        EventBus.Publish(new ActiveVisibleNoteHelper(enableHelper));

        enableTextMesh1.enabled = !enableHelper;
        enableTextMesh2.enabled = enableHelper;
    }
}

public struct ActiveVisibleNoteHelper
{
    public bool isActive;

    public ActiveVisibleNoteHelper(bool value)
    {
        isActive = value;
    }
}