using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CheatMenu : MonoBehaviour
{
    [SerializeField] private List<string> allLevelSceneName = new();
    [SerializeField] private GameObject cheatPanel;

    [SerializeField] private GameObject goToLevelUICard;
    [SerializeField] private GameObject goToUiParent;

    private void Awake()
    {
        SetUiCard();
    }

    public void OpenCheatPanel(bool value)
    {
        cheatPanel.SetActive(value);
    }
    public void SwitchToLevel(int levelID)
    {
        if (levelID >= allLevelSceneName.Count)
            return;

        SceneManager.LoadScene(allLevelSceneName[levelID]);
        OpenCheatPanel(false);
    }
    public void ActiveVisibleNoteHelper(bool value)
    {
        EventBus.Publish(new ActiveVisibleNoteHelper(value));
    }

    public void SetUiCard()
    {
        for (int i = 0; i < allLevelSceneName.Count; i++)
        {
            int levelIndex = i;

            GameObject newCard = Instantiate(goToLevelUICard, goToUiParent.transform);
            newCard.GetComponentInChildren<TextMeshProUGUI>().text = allLevelSceneName[levelIndex];
            newCard.GetComponentInChildren<Button>().onClick.AddListener(() => SwitchToLevel(levelIndex));
        }
    }

}

public struct ActiveVisibleNoteHelper
{
    public bool isActive;
    public ActiveVisibleNoteHelper(bool value)
    {
        isActive = value;
    }
}