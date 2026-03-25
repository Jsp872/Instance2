using AYellowpaper.SerializedCollections;
using Menu;
using UnityEngine;
using UnityEngine.Audio;

public class NoteAudioManager : MonoBehaviour
{
    [Header("Audio clip setter")]
    [SerializeField] SerializedDictionary<NoteID, AudioClip> noteSound = new SerializedDictionary<NoteID, AudioClip>();
    [SerializeField] private AudioSource sources;

    [SerializeField] private AudioClip missSoundClip;
    [SerializeField] private AudioMixer audioMixer;

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
    private void Start()
    {
        ApplyMixerValue(SettingsKeys.General);
        ApplyMixerValue(SettingsKeys.Music);
        ApplyMixerValue(SettingsKeys.Sfx);
    }

    private void ApplyMixerValue(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            audioMixer.SetFloat(key, PlayerPrefs.GetFloat(key));
        }
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
}

public struct OnMissSound { }