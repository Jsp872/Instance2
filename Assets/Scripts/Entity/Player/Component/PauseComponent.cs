using UnityEngine;

public class PauseComponent : PlayerComponent
{
    [SerializeField] private PauseMenu PauseMenu;

    public void OpenPauseMenu()
    {
        PauseMenu.gameObject.SetActive(true);
    }
}
