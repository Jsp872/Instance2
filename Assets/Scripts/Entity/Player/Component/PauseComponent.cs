using UnityEngine;
using UnityEngine.InputSystem;

public class PauseComponent : PlayerComponent
{
    [SerializeField] private PauseMenu PauseMenu;

    public override void HandleInput(InputAction.CallbackContext context)
    {
        PauseMenu.gameObject.SetActive(true);
    }
}
