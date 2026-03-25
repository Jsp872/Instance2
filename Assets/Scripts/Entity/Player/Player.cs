//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private ScreenFade screenFade;
    [SerializeField] private PlayerStatConfig playerStatConfig;
    [SerializeField] private PlayerController controller;

    [SerializeField] private List<AudioClip> deathCatSounds = new();
    [SerializeField] private AudioSource playerAudioSource;


    private int playerLife;
    [SerializeField] private float respawnDelay;
    public float GetRespawnDelay { get => respawnDelay; }

    public event System.Action OnDeath;


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
        playerAudioSource.clip = PickRandomDeathSound();
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
        playerAudioSource.Play();
        yield return new WaitForSeconds(respawnDelay);
        OnDeath?.Invoke();
        Respawn();
    }

    private AudioClip PickRandomDeathSound()
    {
        return deathCatSounds[Random.Range(0, deathCatSounds.Count)];
    }
    private void Respawn()
    {
        screenFade.FadeOut(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), 0.3f);
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