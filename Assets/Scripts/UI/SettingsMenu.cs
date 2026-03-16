using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup settingsPanel;
    [SerializeField] private List<CanvasGroup> settingsPanels;

    private void Start()
    {
        foreach (CanvasGroup panel in settingsPanels)
        {
            panel.Toggle(false);
        }
    }

    public void ReturnToMainMenu()
    {
        settingsPanel.Toggle(false);
    }

    public void OpenPanel(CanvasGroup panel)
    {
        panel.Toggle(true);
        foreach (CanvasGroup currentpanel in settingsPanels)
        {
            if (currentpanel == panel)
            {
                continue;
            }
            currentpanel.Toggle(false);
        }
    }

    public void ClosePanel(CanvasGroup panel)
    {
        panel.Toggle(false);
    }
}
