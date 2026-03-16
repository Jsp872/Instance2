using TMPro;
using UnityEngine;

public class LevelBtn : UI_Basic_Functions
{
    [HideInInspector] public string sceneName;

    public void LoadLevel()
    {
        OpenScene(sceneName);
    }
}
