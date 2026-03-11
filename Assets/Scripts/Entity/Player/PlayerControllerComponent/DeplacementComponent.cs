using UnityEngine;

public class DeplacementComponent : PlayerComponent
{
    private Transform playerT;

    private void Awake()
    {
        playerT = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        playerT.position += (playerConfig.moveSpeedDefaultValue * playerConfig.movementDirection) * Time.deltaTime;
    }
}