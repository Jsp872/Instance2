using UnityEngine;
using UnityEngine.InputSystem;

public class PauseComponent : PlayerComponent
{
    [SerializeField] public PauseMenu PauseMenu;

    public void HandleInput(InputAction.CallbackContext context, bool activePanel)
    {
        PauseMenu.gameObject.SetActive(activePanel);
    }
}
