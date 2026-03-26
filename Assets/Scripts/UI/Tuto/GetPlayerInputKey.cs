using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetPlayerInputKey : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string wishActionName;

    [SerializeField] private GameObject bindingTextGO;
    [SerializeField] private TextMeshPro showBinding;

    [SerializeField] private GameObject jumpPanelImage;
    private void Start()
    {
        InputActionMap map = playerInput.currentActionMap;

        foreach (var action in map.actions)
        {
            string binding = action.GetBindingDisplayString(0);
            if (wishActionName == action.name)
            {
                if(binding == "Espace")
                {
                    SetJumpImage(true);
                    return;
                }
                SetJumpImage(false);
                showBinding.text = binding;
                return;
            }
        }
    }

    private void SetJumpImage(bool value)
    {
        bindingTextGO.SetActive(!value);
        jumpPanelImage.SetActive(value);
        return;
    }
}