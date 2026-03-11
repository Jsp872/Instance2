using UnityEngine;


//[CreateAssetMenu(menuName = "StatConfig", fileName = "EntityStatConfig")]
public class EntityStatConfig : ScriptableObject
{
    [Header("Movement")]
    public Vector3 movementDirection = Vector2.right;
    public LayerMask groundLayer;
    public float moveSpeedDefaultValue;
    [Min(0.0f)] public float minMoveSpeed;
    public float maxMoveSpeed;

    [Min(0.0f)] public float accelerationValue;
    [Min(0.0f)] public float decelerationValue;

    public virtual void OnValidate()
    {
        moveSpeedDefaultValue = Mathf.Clamp(moveSpeedDefaultValue, minMoveSpeed, maxMoveSpeed);
    }
}
