using UnityEngine;
public class BouncingPlatform : BasePlatform
{
    [SerializeField] private float bounceForce = 15f;
    protected override void OnPlayerEnter(PlayerController player)
    {
        // Apply upward force to the player
        player.Bounce(Vector2.up * bounceForce);
    }
}