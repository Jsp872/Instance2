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
            binding = CleanBinding(binding);

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

    string CleanBinding(string binding)
    {
        int index = binding.IndexOf('(');
        if (index > 0)
        {
            return binding.Substring(0, index).Trim();
        }
        return binding;
    }

    private void SetJumpImage(bool value)
    {
        bindingTextGO.SetActive(!value);
        jumpPanelImage.SetActive(value);
        return;
    }
}