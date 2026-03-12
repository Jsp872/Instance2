using TMPro;
using UnityEngine;

public class Level_Button : UI_Basic_Functions
{
    [HideInInspector] public string sceneName;

    public void LoadLevel()
    {
        OpenScene(sceneName);
    }
}
