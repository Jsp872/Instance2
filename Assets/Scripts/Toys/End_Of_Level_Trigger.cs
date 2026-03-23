using System;
using UnityEngine;

public struct OnLevelCompletedCallback { }

public class End_Of_Level_Trigger : MonoBehaviour
{
    [SerializeField] private UI_Basic_Functions VictoryScreen;
    [SerializeField] private float delayBeforeVictoryScreen;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            EventBus.Publish(new OnLevelCompletedCallback());
            Invoke(nameof(TriggerEndOfLevel), delayBeforeVictoryScreen);
        }
    }

    private void TriggerEndOfLevel()
    {
        VictoryScreen.gameObject.SetActive(true);
    }
}
