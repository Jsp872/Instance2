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
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule em;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        em = ps.emission;
        em.enabled = false;
        ps.Play();
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
            ps.startColor = Do;
        }
        else if (note == NoteID.RE)
        {
            ps.startColor = Re;
        }
        else if (note == NoteID.MI)
        {
            ps.startColor = Mi;
        }
        else if (note == NoteID.FA)
        {
            ps.startColor = Fa;
        }
        StartCoroutine(PlayParticles());
    }

    private IEnumerator PlayParticles()
    {
        em.enabled = true;
        yield return new WaitForSeconds(Duration);
        em.enabled = false;
    }
}
