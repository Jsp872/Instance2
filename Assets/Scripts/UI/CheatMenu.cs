using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CheatMenu : UI_Basic_Functions
{
    [SerializeField] private GameObject cheatPanel;
    [SerializeField] private TextMeshProUGUI enableTextMesh1;
    [SerializeField] private TextMeshProUGUI enableTextMesh2;

    bool enableHelper = true;

    private void Awake()
    {
        enableHelper = PlayerBlackboard.Instance.enableVisualHelper;
        ActiveVisibleNoteHelper();
    }
    public void OpenCheatPanel(bool value)
    {
        cheatPanel.SetActive(value);
    }
    public void ActiveVisibleNoteHelper()
    {
        enableHelper = !enableHelper;
        PlayerBlackboard.Instance.enableVisualHelper = enableHelper;
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