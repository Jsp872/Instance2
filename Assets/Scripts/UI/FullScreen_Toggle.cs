using System;
using UnityEngine;
using UnityEngine.UI;

public class FullScreen_Toggle : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Toggle>().isOn = Screen.fullScreen;
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = gameObject.GetComponent<Toggle>().isOn;
    }
}
