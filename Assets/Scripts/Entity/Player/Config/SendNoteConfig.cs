using UnityEngine;

[System.Serializable]
public struct SendNoteConfig : Config
{
    [Tooltip("quand le joueur appuie sur sa touche, attend ce delay avant de commencer a compté le hold de la touche")]
    public float isHoldDelay;

    [Tooltip("si deux touche identique sont jouer dans ce timing alors un combos est jouer")]
    public float combosDelay;

    [Tooltip("hold timer multiplicateur")]
    [Min(0.1f)] public float holdTimerMultiplier;
}