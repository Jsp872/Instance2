using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private EntityStatConfig playerConfig;

    [Header("Player Controller Component")]
    [SerializeField] private PlayerComponent movementComponent;
    [SerializeField] private PlayerComponent jumpComponent;
    [SerializeField] private PlayerComponent interactionComponent;

    private void Awake()
    {
        InitComponents(movementComponent, jumpComponent, interactionComponent);
    }
    private void InitComponents(params PlayerComponent[] components)
    {
        foreach (var component in components)
        {
            if (component != null)
            {
                print($"initialize component : {component}");
                component.Initialize(playerConfig);
            }
        }
    }
}