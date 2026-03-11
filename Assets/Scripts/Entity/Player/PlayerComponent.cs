using UnityEngine;


public class PlayerComponent : MonoBehaviour
{
    protected EntityStatConfig playerConfig;
    public virtual void Initialize(EntityStatConfig playerConfig)
    {
        this.playerConfig = playerConfig;   
    }
    public bool isInputEnable = false;
    public virtual void OnActionStarted() { }
    public virtual void OnActionFinished() { }
}
