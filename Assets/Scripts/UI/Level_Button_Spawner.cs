using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Level_Button_Spawner : MonoBehaviour
{
    [SerializeField] private Level_Button level_Button;
    [SerializeField] private List<string> levels;
    
    void Start()
    {
        foreach (var level in levels)
        {
            Level_Button button = Instantiate(level_Button, transform);
            button.sceneName = level;
            button.GetComponentInChildren<TextMeshProUGUI>().text = level;
        }
    }
}
