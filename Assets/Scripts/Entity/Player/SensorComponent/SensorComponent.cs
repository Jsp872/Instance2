using UnityEngine;
public abstract class SensorComponent : MonoBehaviour
{
    protected PlayerController controller;
    protected Collider2D shapeCollider;

    private int _triggerCount = 0;
    private int _collideCount = 0;
    protected bool IsTriggerActive => _triggerCount > 0;
    protected bool IsCollideActive => _collideCount > 0;
    protected bool IsInLayer(int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;


    public virtual void InitializedSensorComponent(PlayerController controller)
    {
        this.controller = controller;
        shapeCollider = GetComponent<Collider2D>();

        OnResetSensor();
    }
    public virtual void OnResetSensor()
    {
        _triggerCount = 0;
        _collideCount = 0;
    }

    //On Update
    public virtual bool CanUpdateSensor() => false;
    public virtual void OnUpdateSensor(float dT)
    {
        if (!IsTriggerActive || !IsCollideActive)
            return;
    }

    //On Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (controller is null || collision is null) return;

        _triggerCount++;
        TriggerEntry(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (controller is null || collision is null) return;

        _triggerCount = Mathf.Max(0, _triggerCount - 1);
        TriggerExit(collision);
    }
    protected virtual void TriggerEntry(Collider2D collision) { }
    protected virtual void TriggerExit(Collider2D collision) { }

    //On Collisiion 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (controller is null || collision is null) return;

        _collideCount++;
        CollisionEntry(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (controller is null || collision is null) return;

        _collideCount = Mathf.Max(0, _collideCount - 1);
        CollisionExit(collision);
    }
    protected virtual void CollisionEntry(Collision2D collision) { }
    protected virtual void CollisionExit(Collision2D collision) { }
}