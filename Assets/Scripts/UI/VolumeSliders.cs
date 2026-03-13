using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    private PlayerPrefsGestionnary playerPrefs;

    private void Start()
    {
        playerPrefs = gameObject.GetComponent<PlayerPrefsGestionnary>();
    }

    public void ChangeVolume()
    {
        playerPrefs.SetPlayerPrefs(0, (int)gameObject.GetComponent<Slider>().value);
    }
}
