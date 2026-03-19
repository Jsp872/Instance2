using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu
{
    public class SettingsMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject currentPanel;
    
        [Header("Audio")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider sliderGeneral;
        [SerializeField] private Slider sliderMusic;
        [SerializeField] private Slider sliderSfx;

        [Header("Resolution")]
        [SerializeField] private TMP_Dropdown dropdownResolution;

        [Header("Toggle")]
        [SerializeField] private Toggle toggleFullScreen;

        private Resolution[] resolutions;

        private void Start()
        {
            currentPanel.SetActive(true);
            if (dropdownResolution is not null)
            {
                dropdownResolution.ClearOptions();
                AddResolutionOnDropdown(dropdownResolution);
                dropdownResolution.onValueChanged.AddListener(SetResolution);
                dropdownResolution.value = PlayerPrefs.HasKey(SettingsKeys.ResolutionIndex) 
                    ? PlayerPrefs.GetInt(SettingsKeys.ResolutionIndex) : dropdownResolution.options.Count - 1;
            }

            if (PlayerPrefs.HasKey(SettingsKeys.FullScreen))
            {
                Screen.fullScreen = PlayerPrefs.GetInt(SettingsKeys.FullScreen) == 1;
            }
    
            if (toggleFullScreen is not null)
            {
                toggleFullScreen.isOn = Screen.fullScreen;
                toggleFullScreen.onValueChanged.AddListener(SetFullScreen);
            }

            sliderGeneral?.onValueChanged.AddListener(SetVolumeGeneral);
            sliderMusic?.onValueChanged.AddListener(SetVolumeMusic);
            sliderSfx?.onValueChanged.AddListener(SetVolumeSfx);
        }
        private void OnEnable()
        {
            InitAudioSettings(sliderGeneral, SettingsKeys.General);
            InitAudioSettings(sliderMusic, SettingsKeys.Music);
            InitAudioSettings(sliderSfx, SettingsKeys.Sfx);
        }

        public void SetCurrentPanel(GameObject panel)
        {
            currentPanel.SetActive(false);
            currentPanel = panel;
            currentPanel.SetActive(true);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            PlayerPrefs.SetInt(SettingsKeys.FullScreen, isFullScreen ? 1 : 0);
        }

        #region Audio

        private void SetVolumeGeneral(float volume)
        {
            SetVolume(SettingsKeys.General, volume);
        }

        private void SetVolumeSfx(float volume)
        {
            SetVolume(SettingsKeys.Sfx, volume);
        }

        private void SetVolumeMusic(float volume)
        {
            SetVolume(SettingsKeys.Music, volume);
        }

        private void SetVolume(string key, float volume)
        {
            if (audioMixer == null)
            {
                Debug.LogWarning("AudioMixer is not assigned in the inspector.");
                return;
            }
    
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning("Audio key is null or empty.");
                return;
            }
    
            if (volume is < -80.0f or > 20.0f)
            {
                Debug.LogWarning($"Volume value {volume} is out of range. It must be between -80 and 20.");
                return;
            }

            audioMixer.SetFloat(key, volume);
            PlayerPrefs.SetFloat(key, volume);
        }

        private void InitAudioSettings(Slider slider, string key)
        {
            if (slider is null || !PlayerPrefs.HasKey(key) || audioMixer is null)
            {
                return;
            }
        
            slider.value = PlayerPrefs.GetFloat(key);
            audioMixer.SetFloat(key, slider.value);
        }

        #endregion


        #region Resolution

        private void AddResolutionOnDropdown(TMP_Dropdown dropdown)
        {
            if (resolutions is null || resolutions.Length == 0)
            {
                resolutions = Screen.resolutions.GroupBy(r => new { r.width, r.height }).Select(group => group.First()).ToArray();
            }
    
            List<string> options = resolutions.Select(resolution => resolution.width + "x" + resolution.height).ToList();
    
            dropdown.AddOptions(options);
        }

        public void SetResolution(int resolutionIndex)
        {
            if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
            {
                return;
            }
    
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    
            PlayerPrefs.SetInt(SettingsKeys.ResolutionIndex, resolutionIndex);
        }

        #endregion
    }
}