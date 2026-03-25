using System;
using UnityEngine;

public class PauseMenu : UI_Basic_Functions
{
    [Header("Reference")]
    [SerializeField] private GameObject pauseMenus;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ResetBBVariables()
    {
        PlayerBlackboard.Instance.ResetAll();
    }
}
