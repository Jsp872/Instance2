using System.Collections;
using UnityEngine;

public class Destroy_Particles : MonoBehaviour
{
    private ParticleSystem particles;
    private ParticleSystem.EmissionModule emissions;
    [SerializeField] private float ShortDuration;
    [SerializeField] private float BurstLifespan;
    [SerializeField] private float BurstGravity;
    [SerializeField] private float BurstParticlesNumber;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        emissions = particles.emission;
        particles.Play();
        emissions.enabled = false;
    }

    public void ShortActivation()
    {
        emissions.enabled = true;
        StartCoroutine(Stop(ShortDuration));
    }

    public void LongActivation(float duration)
    {
        particles.startLifetime = BurstLifespan;
        particles.gravityModifier = BurstGravity;
        emissions.rateOverTime = BurstParticlesNumber;
        emissions.enabled = true;
        StartCoroutine(Stop(duration));
    }

    private IEnumerator Stop(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        emissions.enabled = false;
    }
}
