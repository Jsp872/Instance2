using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CheatMenu : UI_Basic_Functions
{
    [SerializeField] private GameObject cheatPanel;
    [SerializeField] private TextMeshProUGUI enableTextMesh;
    [SerializeField] private string enableText = "Enable Visual Helper";
    [SerializeField] private string disableText = "Disable Visual Helper";

    bool enableHelper = true;

    private void Awake()
    {
        ActiveVisibleNoteHelper();
    }
    public void OpenCheatPanel(bool value)
    {
        cheatPanel.SetActive(value);
    }
    public void ActiveVisibleNoteHelper()
    {
        enableHelper = !enableHelper;
        EventBus.Publish(new ActiveVisibleNoteHelper(enableHelper));
        enableTextMesh.text = enableHelper ? enableText : disableText;
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