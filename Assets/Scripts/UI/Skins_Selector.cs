using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class Skins_Selector : MonoBehaviour
{
    [SerializeField] private Skin_button skin_button;
    [SerializeField] private Image skinViewer;
    [SerializeField] private List<Image> skins; //for test purposes

    void Start()
    {
        PlayerPrefsGestionnary playerPrefs = gameObject.GetComponent<PlayerPrefsGestionnary>();
        bool firstLoop = true;
        int index = 0;
        foreach (var skin in skins)
        {
            if (firstLoop)
            {
                firstLoop = false;
                if (playerPrefs.GetPlayerPrefs(0) == -1)
                {
                    skinViewer.sprite = skin.sprite;
                }
                else
                {
                    skinViewer.sprite = skins[playerPrefs.GetPlayerPrefs(0)].sprite;
                }
            }
            
            Skin_button button = Instantiate(skin_button, transform);
            button.skins = skins; //for tests purposes
            button.skinViewer = skinViewer;
            button.skinNumber = index;
            button.GetComponentInChildren<Image>().sprite = skin.sprite;
            
            index++;
        }
    }
}
