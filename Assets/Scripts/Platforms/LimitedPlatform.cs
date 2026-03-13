using System.Collections;
using UnityEngine;

public class LimitedPlatform : BasePlatform
{
    [SerializeField] private float disappearDelay = 1f;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private bool autoRespawn;

    private Collider2D platformCollider;
    private Renderer platformRenderer;
    private Coroutine disappearRoutine;
    private bool isActive = true;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        platformRenderer = GetComponent<Renderer>();
    }

    protected override void OnPlayerEnter(PlayerController player)
    {
        if (!isActive || disappearRoutine != null) return;
        disappearRoutine = StartCoroutine(DisappearRoutine());
    }

    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(disappearDelay);
        SetPlatformState(false);

        if (autoRespawn)
        {
            yield return new WaitForSeconds(respawnDelay);
            SetPlatformState(true);
        }

        disappearRoutine = null;
    }

    private void SetPlatformState(bool active)
    {
        isActive = active;
        if (platformCollider != null) platformCollider.enabled = active;
        if (platformRenderer != null) platformRenderer.enabled = active;
    }
}