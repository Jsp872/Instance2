using System;
using UnityEngine;

public class PauseMenu : UI_Basic_Functions
{
    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
