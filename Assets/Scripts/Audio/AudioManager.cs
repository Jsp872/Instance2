using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    public AudioMixer mixer;
    public AudioMixerGroup Master;
    public AudioMixerGroup Music;
    public AudioMixerGroup SFX;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    
    private float timeBetweenMusics;
    private bool resetMusic;

    [HideInInspector] public bool IsPlayingSound;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance) {
            Destroy(gameObject); return;
        }
        
        Instance = this;
        
        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    #region music

    private void Start()
    {
        PlaySound("Music");
    }

    public void RestartMusic()
    {
        resetMusic = true;
        timeBetweenMusics = 0f;
    }

    private void Update()
    {
        
        if (resetMusic)
        {
            PlaySound("Music");
        }
        timeBetweenMusics += Time.deltaTime;
        if (timeBetweenMusics >= GetAudioLength("Music"))
        {
            PlaySound("Music");
        }
    }

    #endregion

    #region sfx

    public void PlaySound(string pName)
    {
        try
        {
            Sound s = Array.Find(Sounds, sound => sound.name == pName);
            s.source.Play();
        }
        catch
        {
            Debug.LogWarning(pName + " sound not found");
        }
    }

    public void PlayOverlap(string pName)
    {
        try
        {
            Sound s = Array.Find(Sounds, sound => sound.name == pName);
            s.source.PlayOneShot(s.source.clip, s.source.volume);
        }
        catch
        {
            Debug.LogWarning(pName + " sound not found");
        }
    }

    public void PlayDelay(string pName, float pDelay)
    {
        StartCoroutine(PlayDelayOverlapCoroutine(pName, pDelay));
    }

    private IEnumerator PlayDelayOverlapCoroutine(string pName, float pDelay)
    {
        yield return new WaitForSeconds(pDelay);
        PlayOverlap(pName);
    }

    public void StopSound(string pName)
    {
        try
        {
            Sound s = Array.Find(Sounds, sound => sound.name == pName);
            s.source.Stop();
        }
        catch
        {
            Debug.LogWarning(pName + " sound not found");
        }
    }

    public void stopAllSounds()
    {
        foreach (Sound s in Sounds)
        {
            if (s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }

    #endregion

    #region utility

    public void ChangeVolume(string pName, float pVolume)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == pName);
        s.source.volume = pVolume;
    }

    public void ChangeMixerVolume(float pVolume, AudioMixerGroup pMixer)
    {
        mixer.SetFloat(pMixer.name, pVolume);
    }
    public void ChangeMasterMixerVolume()
    {
        mixer.SetFloat(Master.name, 0.01f + Mathf.Log10(masterSlider.value) * 20);
    }
    public void ChangeMusicMixerVolume()
    {
        mixer.SetFloat(Music.name, 0.01f + Mathf.Log10(musicSlider.value) * 20);
    }
    public void ChangeSFXMixerVolume()
    {
        mixer.SetFloat(SFX.name, Mathf.Log10(sfxSlider.value) * 20);
    }

    public void ChangePitch(string pName, float pPitch)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == pName);
        s.source.pitch = pPitch;
    }

    public void FadeVolume(string pName, float pDuration, float pTargetVolume)
    {
        StartCoroutine(FadeVolumeCoroutine(pName, pDuration, pTargetVolume));
    }

    private IEnumerator FadeVolumeCoroutine(string pName, float pDuration, float pTargetVolume)
    {
        try
        {
            Sound s = Array.Find(Sounds, sound => sound.name == pName);
            while (Mathf.Approximately(s.source.volume, pTargetVolume))
            {
                s.source.DOFade(pTargetVolume, pDuration);
            }
        }
        catch
        {
            Debug.LogWarning(pName + " sound not found");
        }

        yield return null;
    }

    #endregion

    #region parameters

    public float GetAudioLength(string pName)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == pName);
        return s.source.clip.length;
    }

    public bool IsAudioPlaying(string pName)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == pName);
        return s.source.isPlaying;
    }

    #endregion

    //placer dans n'importe quel scrypt avec le bon nom dans les "" pour jouer un son
    //AudioManager.instance.X(Y);
    //X est le nom de la fonction appelée
    //Y sont les paramètres de X
}