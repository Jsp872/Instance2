using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        FadeIn();
    }
    public void FadeOut(Action callback, float fadeDuration = -1)
    {
        float duration = fadeDuration < -1 ? this.fadeDuration : fadeDuration;
        StartCoroutine(StartFade(true, duration, callback));
    }
    public void FadeIn()
    {
        StartCoroutine(StartFade(false, fadeDuration));
    }

    public IEnumerator StartFade(bool isOut, float fadeDuration, Action callback = null)
    {
        float start = isOut ? 0f : 1f;
        float end = isOut ? 1f : 0f;

        float time = 0f;

        group.alpha = start;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            group.alpha = Mathf.Lerp(start, end, t);

            yield return null;
        }

        group.alpha = end;
        callback?.Invoke();
    }
}