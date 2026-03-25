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

    public void OpenCheatPanel(bool value)
    {
        cheatPanel.SetActive(value);
    }
    public void SwitchToLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
        OpenCheatPanel(false);
    }
    public void ActiveVisibleNoteHelper()
    {
        enableHelper = !enableHelper;

        EventBus.Publish(new ActiveVisibleNoteHelper(enableHelper));
        enableTextMesh.text = enableHelper ? enableText : disableText;
        print(enableHelper);
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