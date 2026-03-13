using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Skin_button : MonoBehaviour
{
    [HideInInspector] public List<Image> skins; //for test purposes
    [HideInInspector] public int skinNumber;
    [HideInInspector] public Image skinViewer;

    private PlayerPrefsGestionnary playerprefs;

    private void Start()
    {
        playerprefs = gameObject.GetComponent<PlayerPrefsGestionnary>();
    }

    public void SelectSkin()
    {
        playerprefs.SetPlayerPrefs(0, skinNumber);
        skinViewer.sprite = skins[skinNumber].sprite;
    }
}
