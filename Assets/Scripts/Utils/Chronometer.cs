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
    }

    public bool CanWriteOnUpdate() => true;
    public void WriteToBB(PlayerBlackboard bb)
    {
        bb.GameChronometer = _chronometer;
    }
    public void ReadFromBB(PlayerBlackboard bb) => _chronometer = bb.GameChronometer;
}