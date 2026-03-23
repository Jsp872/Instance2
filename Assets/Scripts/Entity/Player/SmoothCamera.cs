using System;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [field: SerializeField] public Vector3 offset { get; private set; }
    
    [Header("Speeds")]
    [SerializeField] private float speedX = 10f; 
    [SerializeField] private float speedY = 10f; 
    
    [Tooltip("Distance à laquelle la caméra accroche le joueur")]
    [SerializeField] private float lockThreshold = 0.5f; 

    private float currentSpeedX;
    private float currentSpeedY;
    private Transform currentTarget;
    private bool isLockedOn;

    private void Awake()
    {
        currentSpeedX = speedX;
        currentSpeedY = speedY;
        currentTarget = player;
        EventBus.Subscribe<OnLevelCompletedCallback>(Caca);
    }

    private void Caca(OnLevelCompletedCallback _)
    {
        ChangeTarget(null, 0f, 0f);
    }

    private void FixedUpdate()
    {
        if (currentTarget == null) return;

        Vector3 targetPosition = currentTarget.position + offset;

        if (!isLockedOn)
        {
            if (Vector3.Distance(transform.position, targetPosition) <= lockThreshold)
            {
                isLockedOn = true; 
            }
            else
            {
                return;
            }
        }

        // Déplacement indépendant pour X et Y
        float newX = Mathf.MoveTowards(transform.position.x, targetPosition.x, currentSpeedX * Time.fixedDeltaTime);
        float newY = Mathf.MoveTowards(transform.position.y, targetPosition.y, currentSpeedY * Time.fixedDeltaTime);
        
        // Le Z s'aligne instantanément sur la cible + offset (comportement standard en 2D)
        transform.position = new Vector3(newX, newY, targetPosition.z);
    }

    public void ChangeTarget(Transform newTarget, float newSpeedX, float newSpeedY)
    {
        currentSpeedX = newSpeedX;
        currentSpeedY = newSpeedY;
        currentTarget = newTarget;
        isLockedOn = false; 
    }

    public void FollowPlayer()
    {
        currentSpeedX = speedX;
        currentSpeedY = speedY;
        currentTarget = player;
        isLockedOn = false;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnLevelCompletedCallback>(Caca);
    }
}
