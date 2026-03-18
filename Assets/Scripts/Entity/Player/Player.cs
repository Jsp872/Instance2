using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStatConfig playerStatConfig;
    [SerializeField] private PlayerController controller;
    private void Awake()
    {
        if (controller == null) {
            controller = GetComponent<PlayerController>();
        }

        controller.InitializeComponent(playerStatConfig);
    }
}