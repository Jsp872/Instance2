using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndAnimation : MonoBehaviour
{
    [SerializeField] private EndAnimTrigger end_Of_Level_Trigger;
    [SerializeField] private Player player;
    [SerializeField] private UI_Basic_Functions VictoryScreen;
    [SerializeField] private GameObject Scroll;
    [SerializeField] private float ScrollAnimLenght;
    [SerializeField] private float ScrollAnimHeight;
    private jumpAnim jumpAnim;
    
    void Start()
    {
        end_Of_Level_Trigger.OnEnd_Of_Level_Trigger += AnimMode;
        jumpAnim = GetComponent<jumpAnim>();
        transform.position = new Vector3(transform.position.x, transform.position.y + ScrollAnimHeight, transform.position.z);
    }

    private void AnimMode()
    {
        player.GetComponent<PlayerInput>().enabled = false;
        Scroll.SetActive(true);
    }

    public void ScrollAnim()
    {
        transform.DOMoveY(transform.position.y - ScrollAnimHeight, ScrollAnimLenght).SetEase(Ease.InQuad);
    }

    private void HideScroll()
    {
        Scroll.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            jumpAnim.player = player;
            jumpAnim.Continue += HideScroll;
            StartCoroutine(JumpDelay());
            ScrollAnim();
        }
    }

    private IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(ScrollAnimLenght);
        jumpAnim.enabled = true;
    }
}
