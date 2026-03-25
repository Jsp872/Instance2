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
    public void FadeOut(Action callback)
    {
        StartCoroutine(StartFade(true, callback));
    }
    public void FadeIn()
    {
        StartCoroutine(StartFade(false));
    }

    public IEnumerator StartFade(bool isOut, Action callback = null)
    {
        group.blocksRaycasts = true;

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
        group.blocksRaycasts = false;
        callback?.Invoke();
    }
}