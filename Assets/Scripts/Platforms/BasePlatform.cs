using UnityEngine;

/// <summary>
/// Base class for all platforms that have special behavior when the player lands on them.
/// </summary>
public abstract class BasePlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the Player component
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            // If it's a player, get the controller and trigger the behavior
            if (player.TryGetComponent(out PlayerController controller))
            {
                OnPlayerEnter(controller);
            }
        }
    }

    /// <summary>
    /// Called when the player collides with the platform.
    /// </summary>
    /// <param name="player">The PlayerController of the player.</param>
    protected abstract void OnPlayerEnter(PlayerController player);
}
