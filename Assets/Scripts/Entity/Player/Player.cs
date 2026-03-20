using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    public void SetSpawnPoint(Transform spawnPoint) => this.spawnPoint = spawnPoint;

    [SerializeField] private PlayerStatConfig playerStatConfig;
    [SerializeField] private PlayerController controller;

    private int playerLife;

    [Header("_____DEBUG_____")] [SerializeField]
    private Camera playerCamera;

    private float cameraDefaultFOV;
    [SerializeField] private float FOVvalueToAdd = 10;


    [SerializeField] private float respawnDelay;


    private void Awake()
    {
        cameraDefaultFOV = playerCamera.orthographicSize;
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }

        controller.InitializeComponent(playerStatConfig);
        playerLife = playerStatConfig.playerLife;
    }

    #region TEST_Movement_Event

    private void OnEnable()
    {
        EventBus.Subscribe<OnHitObstacleCallback>(OnHittedWall);
        EventBus.Subscribe<MaxSpeedReachCallback>(OnMaxSpeedReach);
        EventBus.Subscribe<LooseMaxSpeedCallback>(OnLooserMaxSpeed);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnHitObstacleCallback>(OnHittedWall);
        EventBus.Unsubscribe<MaxSpeedReachCallback>(OnMaxSpeedReach);
        EventBus.Unsubscribe<LooseMaxSpeedCallback>(OnLooserMaxSpeed);
    }

    private void OnHittedWall(OnHitObstacleCallback callback)
    {
        StartCoroutine(KillAfterDelay());
    }

    private IEnumerator KillAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        playerLife--;
        if (playerLife <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = spawnPoint.position;
    }

    private void OnMaxSpeedReach(MaxSpeedReachCallback callback)
    {
        print("max speed Reach");
        playerCamera.orthographicSize += FOVvalueToAdd;
    }

    private void OnLooserMaxSpeed(LooseMaxSpeedCallback callback)
    {
        playerCamera.orthographicSize = cameraDefaultFOV;
    }

    #endregion
}