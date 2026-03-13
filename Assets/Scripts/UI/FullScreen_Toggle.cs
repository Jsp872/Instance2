using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FullScreen_Toggle : MonoBehaviour
{
    private PlayerPrefsGestionnary playerPrefs;
    
    void Start()
    {
        playerPrefs = gameObject.GetComponent<PlayerPrefsGestionnary>();
        gameObject.GetComponent<Toggle>().isOn = Screen.fullScreen;
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = gameObject.GetComponent<Toggle>().isOn;

        if (gameObject.GetComponent<Toggle>().isOn)
        {
            playerPrefs.SetPlayerPrefs(0, 1);   
        }
        else
        {
            playerPrefs.SetPlayerPrefs(0, 0);
        }
    }
}
