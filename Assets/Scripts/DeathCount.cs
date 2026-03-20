using UnityEngine;

public class DeathCount : MonoBehaviour
{
    public static DeathCount deathCount;
    [SerializeField] private Player player;
    private int deaths;
    public int Deaths { get => deaths;}

    void Awake()
    {
        if (deathCount == null)
        {
            deathCount = this;
        }
    }

    void Start()
    {
        player.OnDeath += AddDeath;
    }

    private void AddDeath()
    {
        deaths++;
    }
}
