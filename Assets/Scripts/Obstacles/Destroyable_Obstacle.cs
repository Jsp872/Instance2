using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Destroyable_Obstacle : Movement_obstacle
{
    [SerializeField] private Destroy_Particles destroyEffect;
    protected override void UnlockedBehaviour()
    {
        destroyEffect.LongActivation(duration);
        transform.DOMoveY(distance,duration).SetEase(Ease.InQuad);
        base.UnlockedBehaviour();
    }

    protected override void UnLockingBehaviour()
    {
        destroyEffect.ShortActivation();
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
