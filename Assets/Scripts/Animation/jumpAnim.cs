using System;
using System.Collections;
using System.Threading;
using DG.Tweening;
using UnityEngine;

public class jumpAnim : MonoBehaviour
{
    [SerializeField] private float FinalJumpHeight;
    [SerializeField] private float FinalJumpDuration;
    [HideInInspector] public Player player;
    public event Action Continue;

    void Start()
    {
        StartCoroutine(StopPlayer());
        player.transform.DOMoveY(transform.position.y +FinalJumpHeight, FinalJumpDuration);
    }

    private IEnumerator StopPlayer()
    {
        yield return new WaitForSeconds(FinalJumpDuration);
        Continue?.Invoke();
        enabled = false;
    }
}
