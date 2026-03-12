using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    protected PlayerStatConfig playerConfig;
    protected Rigidbody2D rb;
    public virtual void Initialize(PlayerStatConfig config, Rigidbody2D rb)
    {
        playerConfig = config;
        this.rb = rb;
    }
    public virtual void OnUpdated(ref Vector3 velocity, float fixedDeltaTime) { }
    public virtual void OnActionStarted() { }
    public virtual void OnActionCanceled() { }
}