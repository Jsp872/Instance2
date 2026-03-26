using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackboard : MonoBehaviour
{
    public static PlayerBlackboard Instance { get; private set; }

    public float GameChronometer { get; set; }
    public int PlayerDeaths { get; set; }

    public bool enableVisualHelper { get; set; } = true;

    [Header("Collect Delay")]
    private readonly List<IBBContributor> _contributors = new();
    [SerializeField] private float collectIntervalSeconds = 1f;
    private float _timer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void ResetAll()
    {
        GameChronometer = 0.0f;
        PlayerDeaths = 0;
    }
    public void Register(IBBContributor contributor) => _contributors.Add(contributor);
    public void Unregister(IBBContributor contributor) => _contributors.Remove(contributor);

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < collectIntervalSeconds) return;

        _timer = 0f;
        CollectAll();
    }

    private void CollectAll()
    {
        foreach (var contributor in _contributors)
        {
            if (!contributor.CanWriteOnUpdate()) continue;

            contributor.WriteToBB(this);
        }
    }
}