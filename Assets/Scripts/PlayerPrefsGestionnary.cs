using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsGestionnary : MonoBehaviour
{
    [SerializeField] private List<string> keys;

    public void SetPlayerPrefs(int key,int value)
    {
        PlayerPrefs.SetInt(keys[key], value);
    }

    public int GetPlayerPrefs(int key)
    {
        if (PlayerPrefs.HasKey(keys[key]))
        {
            return PlayerPrefs.GetInt(keys[key]);
        }
        else
        {
            return -1;
        }
    }
}
