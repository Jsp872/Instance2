using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerComponent : MonoBehaviour
{
    protected PlayerController playerController;
    public virtual void Initialize(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public virtual bool CanUpdate() => true;
    public virtual void UpdateComponent(ref Vector3 velocity, float dT)
    {
        if (!CanUpdate())
            return;
    }

    public virtual void HandleInput<T>(InputAction.CallbackContext ctx, T param) { }
    public virtual void HandleInput(InputAction.CallbackContext ctx) { }
}