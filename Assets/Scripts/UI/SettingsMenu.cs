using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup settingsPanel;
    public void ReturnToMainMenu()
    {
        settingsPanel.Toggle(false);
    }
}
