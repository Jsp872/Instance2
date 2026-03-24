// DeathCount.cs
using UnityEngine;

public class DeathCount : MonoBehaviour, IBBContributor
{
    public static DeathCount Instance;
    [SerializeField] private Player player;
    public int Deaths { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        player.OnDeath += HandleDeath;
        ((IBBContributor)this).Register();
    }

    private void OnDisable()
    {
        player.OnDeath -= HandleDeath;
        ((IBBContributor)this).Unregister();
    }

    private void HandleDeath()
    {
        Deaths++;
        WriteToBB(PlayerBlackboard.Instance);
    }

    public bool CanWriteOnUpdate() => false;
    public void WriteToBB(PlayerBlackboard bb)
    {
        bb.PlayerDeaths = Deaths;
        bb.death = bb.PlayerDeaths;
    }
    public void ReadFromBB(PlayerBlackboard bb) => Deaths = bb.PlayerDeaths;
}