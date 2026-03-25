using UnityEngine;
using UnityEngine.InputSystem;

public class PauseComponent : PlayerComponent
{
    [SerializeField] private PauseMenu PauseMenu;
    private bool isActive = false;

    public override void HandleInput(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        PauseMenu.gameObject.SetActive(isActive);
    }
}
