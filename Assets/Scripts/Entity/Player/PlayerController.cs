using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debugLogs = false;

    [Header("Sensors")]
    public GroundSensor groundSensor;
    public FarObstacleSensor farObstacleSensor;
    public NearObstacleSensor nearObstacleSensor;

    private PlayerStatConfig playerConfig;
    private Rigidbody2D rb;

    // Composants updatés chaque frame
    private readonly List<PlayerComponent> updatableComponents = new();

    // Composants spécifiques gardés pour le dispatch d'input
    private JumpComponent jumpComponent;
    private SendNoteComponent sendNoteComponent;
    private PauseComponent pauseComponent;
    private AutoMoveComponent autoMoveComponent;

    public PlayerStatConfig GetConfig() => playerConfig;
    public Rigidbody2D GetRb() => rb;

    private void Update()
    {
        float dt = Time.deltaTime;
        var velocity = Vector3.zero;

        UpdateSensors(dt, groundSensor, farObstacleSensor, nearObstacleSensor);
        UpdateComponents(ref velocity, dt);

        rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocityY);

        Log($"[PlayerController] velocity={velocity}");
    }

    #region component Life cycle
    public void InitializeComponent(PlayerStatConfig config)
    {
        playerConfig = config;

        rb = GetComponent<Rigidbody2D>();

        InitSensorsInChildren();

        AddPlayerComponent(out jumpComponent, isUpdatableComp: true);
        AddPlayerComponent(out sendNoteComponent);
        AddPlayerComponent(out pauseComponent);
        AddPlayerComponent(out autoMoveComponent, isUpdatableComp: true);

        Log("[PlayerController] Composants initialisés.");
    }
    public void OnResetComponent()
    {
        foreach (PlayerComponent comp in updatableComponents)
            comp.OnPlayerRespawn();

        ResetSensors(groundSensor, farObstacleSensor, nearObstacleSensor);
    }
    public void DisableAllComponent()
    {
        DisableSensor(groundSensor, nearObstacleSensor, farObstacleSensor);
        DisablePlayerComponents(autoMoveComponent, jumpComponent, sendNoteComponent, pauseComponent);
    }
    private void DisablePlayerComponents(params PlayerComponent[] components)
    {
        for(int i = 0; i < components.Length; i++)
        {
            components[i].enabled = false;
        }
    }
    private void UpdateComponents(ref Vector3 velocity, float dt)
    {
        foreach (var comp in updatableComponents)
        {
            if (!comp.CanUpdate()) continue;
            comp.UpdateComponent(ref velocity, dt);
            Log($"[PlayerController] UpdateComp: {comp.GetType().Name}");
        }
    }

    #endregion

    #region Sensor Life cycle
    private void InitSensorsInChildren()
    {
        SensorComponent[] sensors = GetComponentsInChildren<SensorComponent>();
        if (sensors.Length == 0)
        {
            Debug.LogWarning("[PlayerController] Aucun SensorComponent trouvé.", this);
            return;
        }

        foreach (SensorComponent sensor in sensors)
        {
            sensor.InitializedSensorComponent(this);
            Log($"[PlayerController] Sensor initialisé: {sensor.GetType().Name}");
        }
    }
    private void ResetSensors(params SensorComponent[] sensors)
    {
        foreach (var sensor in sensors)
            sensor.OnResetSensor();
    }
    private void DisableSensor(params SensorComponent[] sensors)
    {
        for (int i = 0; i < sensors.Length; i++)
        {
            sensors[i].enabled = false;
        }
    }
    private void UpdateSensors(float dt, params SensorComponent[] sensors)
    {
        foreach (SensorComponent sensor in sensors)
        {
            if (sensor == null || !sensor.CanUpdateSensor()) return;
            sensor.OnUpdateSensor(dt);
        }
    }

    #endregion

    #region Input Field
    public void OnJump(InputAction.CallbackContext ctx)
    {
        Log("[PlayerController] OnJump");
        jumpComponent.HandleInput(ctx);
    }
    public void OnOpenPauseSetting(InputAction.CallbackContext ctx)
    {
        Log("[PlayerController] OnOpenPauseSetting");
        pauseComponent.HandleInput(ctx);
    }
    public void OnSendDO(InputAction.CallbackContext ctx)
    {
        Log("[PlayerController] OnSendDO");
        sendNoteComponent.HandleInput(ctx, NoteID.DO);
    }
    public void OnSendRE(InputAction.CallbackContext ctx)
    {
        Log("[PlayerController] OnSendRE");
        sendNoteComponent.HandleInput(ctx, NoteID.RE);
    }
    public void OnSendMI(InputAction.CallbackContext ctx)
    {
        Log("[PlayerController] OnSendMI");
        sendNoteComponent.HandleInput(ctx, NoteID.MI);
    }
    public void OnSendFA(InputAction.CallbackContext ctx)
    {
        Log("[PlayerController] OnSendFA");
        sendNoteComponent.HandleInput(ctx, NoteID.FA);
    }
    #endregion

    public void Bounce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
        Log($"[PlayerController] Bounce: force={force}");
    }
    private void AddPlayerComponent<T>(out T pComponent, bool isUpdatableComp = false) where T : PlayerComponent
    {
        if (!TryGetComponent(out pComponent))
        {
            pComponent = gameObject.AddComponent<T>();
        }

        pComponent.Initialize(this);

        if (isUpdatableComp)
        {
            updatableComponents.Add(pComponent);
        }

        Log($"[PlayerController] PlayerComponent initialisé: {typeof(T).Name}");
    }
    private void Log(string msg)
    {
        if (debugLogs) Debug.Log(msg, this);
    }
}