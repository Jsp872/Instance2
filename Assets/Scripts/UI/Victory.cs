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

        int minutes = (int)(totalLevelTimer / 60f);
        int seconds = (int)(totalLevelTimer % 60f);
        int milliseconds = Mathf.FloorToInt((totalLevelTimer * 100f) % 100f);
        System.TimeSpan time = System.TimeSpan.FromSeconds(totalLevelTimer);

        totalTimer_TEXT.text = $"{time.Minutes:00}:{time.Seconds:00}:{time.Milliseconds / 10:00}";
    }



    private void OnEnable()
    {
        //Time.timeScale = 0;
        InitText();
    }

    private void OnDisable()
    {
        //Time.timeScale = 1;
    }
}
