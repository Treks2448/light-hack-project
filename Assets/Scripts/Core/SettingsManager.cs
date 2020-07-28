using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Core
{
    // This class handles setting, saving and loading user settings from the options menu.
    public class SettingsManager : MonoBehaviour
    {
        // Varialbes visible in editor
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private Dropdown resolutionDropdown;
        [SerializeField] private Dropdown pResolutionDropdown;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Slider pVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider pMusicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider pSfxVolumeSlider;
        [SerializeField] private Toggle fullScreenToggle;

        // Private variables
        private Resolution[] resolutions;
        private int screenInt;
        private const string resName = "resolutionValue";

        // Unity Functions
        void Awake()
        {
            resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
            {
                PlayerPrefs.SetInt(resName, resolutionDropdown.value);
                PlayerPrefs.Save();
            }));
        }

        void Start()
        {
            // Check whether the game is fullscreen or not
            screenInt = PlayerPrefs.GetInt("toggleState");
            if (screenInt == 1) { fullScreenToggle.isOn = true; }
            else { fullScreenToggle.isOn = false; }

            // Load saved sound options
            volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
            pVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
            mixer.SetFloat("Master Volume", volumeSlider.value);

            musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
            pMusicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
            mixer.SetFloat("Music Volume", musicVolumeSlider.value);

            sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            pSfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            mixer.SetFloat("SFX Volume", sfxVolumeSlider.value);

            LoadScreenOptions();
        }


        // Public functions ///////////////////////////
        
        // Audio Settings 
        public void SetVolume(float volume)
        {
            mixer.SetFloat("Master Volume", volume);
            PlayerPrefs.SetFloat("masterVolume", volume);
            volumeSlider.value = volume;
            pVolumeSlider.value = volume;
        }

        public void SetSFXVolume(float volume)
        {
            mixer.SetFloat("SFX Volume", volume);
            PlayerPrefs.SetFloat("sfxVolume", volume);
            sfxVolumeSlider.value = volume;
            pSfxVolumeSlider.value = volume;
        }

        public void SetMusicVolume(float volume)
        {
            mixer.SetFloat("Music Volume", volume);
            PlayerPrefs.SetFloat("musicVolume", volume);
            musicVolumeSlider.value = volume;
            pMusicVolumeSlider.value = volume;
        }

        // Screen Settings
        public void SetFullscreen(bool fullScreen)
        {
            int val;
            if (fullScreen) val = 1; else val = 0;
            Screen.fullScreen = fullScreen;
            PlayerPrefs.SetInt("toggleState", val);
            PlayerPrefs.Save();
        }

        public void SetResolution(int index)
        {
            Screen.SetResolution(resolutions[index].width, 
                resolutions[index].height, 
                Screen.fullScreen, resolutions[index].refreshRate);
        }

        // Private Functions ////////////////////////////
        private void LoadScreenOptions()
        {
            // TODO: load the saved options instead of the default options
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            pResolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                /*
                if (resolutions[i].refreshRate != 60 && 
                    resolutions[i].refreshRate != 59 &&
                    resolutions[i].refreshRate != 120 &&
                    resolutions[i].refreshRate != 144)
                { continue; }
                */
                string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "Hz";
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            pResolutionDropdown.AddOptions(options);
            pResolutionDropdown.value = currentResolutionIndex;
            pResolutionDropdown.RefreshShownValue();
        }
    }
}

