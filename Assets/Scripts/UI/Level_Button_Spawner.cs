using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Level_Button_Spawner : MonoBehaviour
{
    [SerializeField] private LevelBtn levelBtnPrefab;
    [SerializeField] private List<string> levels;
    [SerializeField] private RectTransform parent;
    
    void Start()
    {
        foreach (var level in levels)
        {
            LevelBtn btn = Instantiate(levelBtnPrefab, parent);
            btn.sceneName = level;
            btn.GetComponentInChildren<TextMeshProUGUI>().text = level;
            
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
    }
    
    
}
