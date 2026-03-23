using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerComponent : MonoBehaviour
{
    protected PlayerController Controller { get; private set; }
    protected PlayerStatConfig Config { get; private set; }
    protected bool debugLogs = false;

    public virtual void Initialize(PlayerController controller)
    {
        Controller = controller;
        Config = controller.GetConfig();
    }

    public virtual void OnPlayerRespawn() { }
    public virtual bool CanUpdate() => enabled;

    public virtual void UpdateComponent(ref Vector3 velocity, float dt) { }
    public virtual void HandleInput(InputAction.CallbackContext ctx) { }

    public void Log(string message) { if (debugLogs) Debug.Log(message); }
}