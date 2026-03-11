using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    private PlayerController controller;
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
}