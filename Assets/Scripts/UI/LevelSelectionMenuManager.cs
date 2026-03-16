using UnityEngine;

public class LevelSelectionMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup levelSelectorMenu;
    public void GoBackToMainPanel()
    {
        levelSelectorMenu.Toggle(false);
    }
}
