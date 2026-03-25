using System;
using UnityEngine;

public class EndAnimTrigger : MonoBehaviour
{
    public event Action OnEnd_Of_Level_Trigger;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            OnEnd_Of_Level_Trigger?.Invoke();
        }
    }
}
