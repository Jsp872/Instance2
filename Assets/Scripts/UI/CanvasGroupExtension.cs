using System.Collections;
using UnityEngine;

public static class CanvasGroupExtension
{
    public static void Toggle(this CanvasGroup canvasGroup, bool value, float duration = 0.3f)
    {
        CoroutineRunner.Instance.StartCoroutine(FadeRoutine(canvasGroup, value, duration));
    }

    private static IEnumerator FadeRoutine(CanvasGroup canvasGroup, bool value, float duration)
    {
        float from = canvasGroup.alpha;
        float to = value ? 1f : 0f;

        if (value)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
            canvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        canvasGroup.alpha = to;

        if (!value)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}

internal class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance != null) return _instance;
            var go = new GameObject("[CoroutineRunner]");
            _instance = go.AddComponent<CoroutineRunner>();
            DontDestroyOnLoad(go);
            return _instance;
        }
    }
}