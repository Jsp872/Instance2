using UnityEngine;


public class EntityStatConfig : ScriptableObject
{
    [Space(10)]
    [Header("Movement")]
    [Tooltip("Direction du perso dans la scene, par default -> droite")]
    public Vector3 movementDirection = Vector2.right;

    [Tooltip("Layer du sol, utiliser pour les collisions et le saut")]
    public LayerMask groundLayer;

    [Tooltip("vitesse initial du perso")]
    public float moveSpeedDefaultValue;

    [Tooltip("vitesse minimum du perso")]
    [Min(0.0f)] public float minMoveSpeed;
    [Tooltip("vitesse maximum du perso")]
    public float maxMoveSpeed;

    [Tooltip("valeur de d'acceleration appliquer a la vitesse a chaque frame")]
    [Min(0.0f)] public float accelerationValue;
    [Tooltip("valeur de deceleration appliquer a la vitesse a chaque frame")]
    [Min(0.0f)] public float decelerationValue;

    [Tooltip("determine a quelle distance d'un obstacle il va commencé a descelerer")]
    public float decelerationDistance;

    public float gravityValue = 9.81f;

    public virtual void OnValidate()
    {
        moveSpeedDefaultValue = Mathf.Clamp(moveSpeedDefaultValue, minMoveSpeed, maxMoveSpeed);
    }
}
