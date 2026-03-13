using System;
using UnityEngine;

public class End_Of_Level_Trigger : MonoBehaviour
{
    [SerializeField] private UI_Basic_Functions VictoryScreen;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            VictoryScreen.gameObject.SetActive(true);
        }
    }
}
