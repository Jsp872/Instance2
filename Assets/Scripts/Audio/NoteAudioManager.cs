using AYellowpaper.SerializedCollections;
using UnityEngine;

public class NoteAudioManager : MonoBehaviour
{
    [Header("Audio clip setter")]
    [SerializeField] SerializedDictionary<NoteID, AudioClip> noteSound = new SerializedDictionary<NoteID, AudioClip>();
    [SerializeField] private AudioSource sources;

    #region Events  
    private void OnEnable()
    {
        EventBus.Subscribe<OnSendNoteSound>(PlaySound);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<OnSendNoteSound>(PlaySound);
    }
    #endregion

    private void PlaySound(OnSendNoteSound callback)
    {
        sources.clip = noteSound[callback.id];
        sources.Play();
    }
}