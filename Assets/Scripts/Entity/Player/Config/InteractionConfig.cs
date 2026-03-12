using UnityEngine;

[System.Serializable]
public struct InteractionConfig : Config
{
    [Header("Interaction")]
    public LayerMask interactibleLayer;
    public float interactionRange;
}