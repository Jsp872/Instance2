using System;
using System.Collections;
using UnityEngine;

public class NoteAuras : MonoBehaviour
{
    [SerializeField] private float Duration;
    [SerializeField] private Color Do;
    [SerializeField] private Color Re;
    [SerializeField] private Color Mi;
    [SerializeField] private Color Fa;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<NoteID>(OnNotePlayed);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<NoteID>(OnNotePlayed);
    }

    private void OnNotePlayed(NoteID note)
    {
        print(note);
        if (note == NoteID.DO)
        {
            spriteRenderer.color = Do;
        }
        else if (note == NoteID.RE)
        {
            spriteRenderer.color = Re;
        }
        else if (note == NoteID.MI)
        {
            spriteRenderer.color = Mi;
        }
        else if (note == NoteID.FA)
        {
            spriteRenderer.color = Fa;
        }
        StartCoroutine(PlayParticles());
    }

    private IEnumerator PlayParticles()
    {
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(Duration);
        spriteRenderer.enabled = false;
    }
}
