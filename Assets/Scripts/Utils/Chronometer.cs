using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

public class Chronometer : MonoBehaviour
{
    [SerializeField] private float startingTime = 60f;
    [SerializeField] private float timeWhenTimerChange = 10f;
    
    [SerializeField] private TMP_Text timerText;

    private float timeLeft;

    private void Start()
    {
        timeLeft = startingTime;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        timeLeft = Mathf.Max(timeLeft, 0);

        UpdateTimerUI();

        if (timeLeft <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void UpdateTimerUI()
    {
        int seconds = (int)timeLeft;
        if (timeLeft <= timeWhenTimerChange)
        {
            int milliseconds = Mathf.FloorToInt((timeLeft * 100) % 100);
            timerText.text = $"{seconds}.{milliseconds:00}";
        }
        else
        {
            timerText.text = $"{seconds}";
        }

    }
}