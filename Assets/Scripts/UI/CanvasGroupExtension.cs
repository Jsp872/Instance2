using UnityEngine;


public static class CanvasGroupExtension
{
    public static void Toggle(this CanvasGroup canvasGroup, bool value)
    {
        canvasGroup.alpha = value ? 1 : 0;
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }
}