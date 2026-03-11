using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class JumpComponent : PlayerComponent
{
    public bool isGrounded;


    [SerializeField] private Transform playerFeetPosition;
    [SerializeField] private float groundDetectionRadius;

    public override void Initialize(EntityStatConfig playerConfig)
    {
        base.Initialize(playerConfig);

        print(this.playerConfig == null ? "null" : "init");
        if (this.playerConfig is null) enabled = false;
    }


    public void OnJump(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            if (!CheckIsGrounded())
                return;
            print("Player jump");
        }
    }

    public bool CheckIsGrounded()
    {
        bool isGrounded = Physics2D.Raycast(playerFeetPosition.position, Vector2.down, groundDetectionRadius, playerConfig.groundLayer);
        Debug.DrawLine(playerFeetPosition.position, playerFeetPosition.position + (groundDetectionRadius * Vector3.down));

        return isGrounded;
    }
}