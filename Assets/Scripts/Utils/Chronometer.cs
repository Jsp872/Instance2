// Chronometer.cs
using TMPro;
using UnityEngine;

public class Chronometer : MonoBehaviour, IBBContributor
{
    [SerializeField] private TMP_Text timerText;
    private float _chronometer;

    private void Start() => ((IBBContributor)this).Register();
    private void OnDestroy() => ((IBBContributor)this).Unregister();

    private void Update()
    {
        _chronometer += Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int seconds = (int)_chronometer;
        int milliseconds = Mathf.FloorToInt((_chronometer * 100f) % 100f);
        timerText.text = $"{seconds}.{milliseconds:00}";
    }

    public bool CanWriteOnUpdate() => true;
    public void WriteToBB(PlayerBlackboard bb)
    {
        bb.GameChronometer = _chronometer;
        bb.chronos = bb.GameChronometer;
    }
    public void ReadFromBB(PlayerBlackboard bb) => _chronometer = bb.GameChronometer;
}