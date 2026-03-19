using UnityEngine;
using static AutoMoveComponent;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStatConfig playerStatConfig;
    [SerializeField] private PlayerController controller;
    [SerializeField] private Camera playerCamera;

    private float cameraDefaultFOV;
    [SerializeField] private float FOVvalueToAdd = 10;
    private void Awake()
    {
        cameraDefaultFOV = playerCamera.orthographicSize;
        if (controller == null) {
            controller = GetComponent<PlayerController>();
        }

        controller.InitializeComponent(playerStatConfig);
    }
    #region TEST_Movement_Event
    private void OnEnable()
    {
        EventBus.Subscribe<HitObstacleCallback>(OnHittedWall);
        EventBus.Subscribe<MaxSpeedReachCallback>(OnMaxSpeedReach);
        EventBus.Subscribe<LooseMaxSpeedCallback>(OnLooserMaxSpeed);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<HitObstacleCallback>(OnHittedWall);
        EventBus.Unsubscribe<MaxSpeedReachCallback>(OnMaxSpeedReach);
        EventBus.Unsubscribe<LooseMaxSpeedCallback>(OnLooserMaxSpeed);
    }

    private void OnHittedWall(HitObstacleCallback callback)
    {
        print("PLAYER DEATH");
        gameObject.SetActive(false);
    }
    private void OnMaxSpeedReach(MaxSpeedReachCallback callback)
    {
        print("max speed Reach");
        playerCamera.orthographicSize += FOVvalueToAdd;
    }
    private void OnLooserMaxSpeed(LooseMaxSpeedCallback callback)
    {
        print("loose max speed");
        playerCamera.orthographicSize = cameraDefaultFOV;
    }
    #endregion
}