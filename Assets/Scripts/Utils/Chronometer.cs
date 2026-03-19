using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

public class Chronometer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private float chronometer;

    private void Update()
    {
        chronometer += Time.deltaTime;
        chronometer = Mathf.Max(chronometer, 0);

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int seconds = (int)chronometer;
        int milliseconds = Mathf.FloorToInt((chronometer * 100) % 100);
        timerText.text = $"{seconds}.{milliseconds:00}";
    }
}