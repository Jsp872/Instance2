using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    private PlayerController controller;
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        //controller.InitializeComponent();
    }

    //private void FixedUpdate()
    //{
    //    Vector3 velocity = Vector3.zero;
    //    controller.UpdateComponent(ref velocity, Time.fixedDeltaTime);

    //    transform.position += velocity;
    //}
}