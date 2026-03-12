using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropDown : MonoBehaviour
{
    [SerializeField] List<Vector2> resolutions;
    [SerializeField] TMP_Dropdown dropdown;

    private void Awake()
    {
        foreach (var res in resolutions)
        {
            if (Screen.currentResolution.Equals(res))
            {
                dropdown.value = resolutions.IndexOf(res);
                break;
            }
        }
    }

    public void ChangeScreenResolution()
    {
        Screen.SetResolution((int)resolutions[dropdown.value].x, (int)resolutions[dropdown.value].y, Screen.fullScreen);
    }
}
