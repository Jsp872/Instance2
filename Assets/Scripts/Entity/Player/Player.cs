using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ScreenFade screenFade;
    public void SetSpawnPoint(Transform spawnPoint) => this.spawnPoint = spawnPoint;

    [SerializeField] private PlayerStatConfig playerStatConfig;
    [SerializeField] private PlayerController controller;

    private int playerLife;
    [SerializeField] private float respawnDelay;
    public float GetRespawnDelay { get => respawnDelay; }

    public event Action OnDeath;


    private void Awake()
    {
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }
        if (screenFade is null)
        {
            screenFade = FindFirstObjectByType<ScreenFade>();
        }

        controller.InitializeComponent(playerStatConfig);
        playerLife = playerStatConfig.playerLife;
    }

    #region TEST_Movement_Event

    private void OnEnable()
    {
        EventBus.Subscribe<OnHitObstacleCallback>(OnHittedWall);
        //EventBus.Subscribe<MaxSpeedReachCallback>(OnMaxSpeedReach);
        //EventBus.Subscribe<LooseMaxSpeedCallback>(OnLooserMaxSpeed);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnHitObstacleCallback>(OnHittedWall);
        //EventBus.Unsubscribe<MaxSpeedReachCallback>(OnMaxSpeedReach);
        //EventBus.Unsubscribe<LooseMaxSpeedCallback>(OnLooserMaxSpeed);
    }

    private void OnHittedWall(OnHitObstacleCallback callback)
    {
        StartCoroutine(KillAfterDelay());
        DisableAllComponent();
    }

    private void DisableAllComponent()
    {
        controller.DisableAllComponent(false);
        controller.enabled = false;
    }

    private IEnumerator KillAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        //playerLife--;
        //if (playerLife <= 0)
        //{
        OnDeath?.Invoke();
        Respawn();
        //}
    }

    private void Respawn()
    {
        screenFade.FadeOut(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        //transform.position = spawnPoint.position;
        //controller.OnResetComponent();
    }

    //private void OnMaxSpeedReach(MaxSpeedReachCallback callback)
    //{
    //    print("max speed Reach");
    //}

    //private void OnLooserMaxSpeed(LooseMaxSpeedCallback callback)
    //{
    //    print("max speed loose");
    //}

    #endregion
}