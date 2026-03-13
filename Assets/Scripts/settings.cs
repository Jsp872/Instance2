using System;
using UnityEngine;

public class settings : MonoBehaviour
{
    private PlayerPrefsGestionnary playerPrefs;
    private void Awake()
    {
        playerPrefs = gameObject.GetComponent<PlayerPrefsGestionnary>();

        //applies settings
        if (playerPrefs.GetPlayerPrefs(0) == 1)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }

        if (playerPrefs.GetPlayerPrefs(1) != -1)
        {
            Screen.SetResolution(playerPrefs.GetPlayerPrefs(0), playerPrefs.GetPlayerPrefs(1), Screen.fullScreen);
        }
    }
}
