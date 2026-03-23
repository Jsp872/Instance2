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


    public void OnResetComponent()
    {
        foreach (PlayerComponent comp in updatableComponents)
            comp.OnPlayerRespawn();

        ResetSensors(groundSensor, farObstacleSensor, nearObstacleSensor);
    }

    private void ResetSensors(params SensorComponent[] sensors)
    {
        foreach (var sensor in sensors)
            sensor.OnResetSensor();
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        var velocity = Vector3.zero;

        UpdateSensors(dt, groundSensor, farObstacleSensor, nearObstacleSensor);
        UpdateComponents(ref velocity, dt);

        rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocityY);

        Log($"[PlayerController] velocity={velocity}");
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

    private void UpdateSensors(float dt, params SensorComponent[] sensors)
    {
        foreach (SensorComponent sensor in sensors)
        {
            if (sensor == null || !sensor.CanUpdateSensor()) return;
            sensor.OnUpdateSensor(dt);
        }
    }
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