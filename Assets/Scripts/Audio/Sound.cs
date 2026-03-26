using UnityEngine;
using UnityEngine.Audio;
namespace AUDIO_PACKAGE
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;
        public AudioMixerGroup audioMixerGroup; //can be null

        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch; //set to 1 for normal sound
        public bool loop;
        public bool playOnAwake;

        [HideInInspector]
        public AudioSource source;
    }
}