using TMPro;
using UnityEngine;

public class Victory : UI_Basic_Functions
{
    [Header("reference")]
    [SerializeField] private TextMeshProUGUI totalTimer_TEXT;
    [SerializeField] private TextMeshProUGUI totalPlayerDeath_TEXT;


    private void InitText()
    {
        int playerDeath = PlayerBlackboard.Instance.PlayerDeaths;
        totalPlayerDeath_TEXT.text = playerDeath.ToString();


        float totalLevelTimer = PlayerBlackboard.Instance.GameChronometer;
        int seconds = (int)totalLevelTimer;
        int milliseconds = Mathf.FloorToInt((totalLevelTimer * 100f) % 100f);
        totalTimer_TEXT.text = $"{seconds}.{milliseconds:00}";
    }



    private void OnEnable()
    {
        Time.timeScale = 0;
        InitText();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
