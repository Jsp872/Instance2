using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup levelSelectorMenu;
    [SerializeField] private CanvasGroup settingsPanel;

    private void Start()
    {
        levelSelectorMenu.Toggle(false);
        settingsPanel.Toggle(false);
    }

    public void Play()
    {
        levelSelectorMenu.Toggle(true);
    }

    public void OpenSkin()
    {
    }

    public void OpenOptions()
    {
        settingsPanel.Toggle(true);
    }

    public void OpenCredits()
    {
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}