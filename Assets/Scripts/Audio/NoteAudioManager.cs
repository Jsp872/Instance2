using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class NoteAudioManager : MonoBehaviour
{
    [Header("Audio clip setter")]
    [SerializeField] SerializedDictionary<NoteID, AudioClip> noteSound = new SerializedDictionary<NoteID, AudioClip>();
    [SerializeField] private AudioSource sources;

    [SerializeField] private AudioClip missSoundClip;


    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup master;
    [SerializeField] private AudioMixerGroup music;
    [SerializeField] private AudioMixerGroup sfx;

    [Header("Volume slider ref")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    #region Events  
    private void OnEnable()
    {
        EventBus.Subscribe<OnSendNoteSound>(PlaySound);
        EventBus.Subscribe<OnMissSound>(MissSound);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<OnSendNoteSound>(PlaySound);
        EventBus.Unsubscribe<OnMissSound>(MissSound);
    }
    #endregion

    private void PlaySound(OnSendNoteSound callback)
    {
        sources.clip = noteSound[callback.id];
        sources.Play();
    }
    private void MissSound(OnMissSound callback)
    {
        sources.clip = missSoundClip;
        sources.Play();
    }

    #region utility

    public void ChangeVolume(float pVolume)
    {
        sources.volume = pVolume;
    }
    public void ChangeMixerVolume(float pVolume, AudioMixerGroup pMixer)
    {
        mixer.SetFloat(pMixer.name, pVolume);
    }
    public void ChangeMasterMixerVolume()
    {
        mixer.SetFloat(master.name, 0.01f + Mathf.Log10(masterSlider.value) * 20);
    }
    public void ChangeMusicMixerVolume()
    {
        mixer.SetFloat(music.name, 0.01f + Mathf.Log10(musicSlider.value) * 20);
    }
    public void ChangeSfxMixerVolume()
    {
        mixer.SetFloat(sfx.name, Mathf.Log10(sfxSlider.value) * 20);
    }

    #endregion
}

public struct OnMissSound { }