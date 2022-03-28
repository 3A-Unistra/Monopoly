/*
 * MenuOptions.cs
 * This file contain the event listeners of the options' Menu buttons, slider and
 * switches.
 * 
 * Date created : 27/02/2022
 * Author       : Rayan MARMAR <rayan.marmar@etu.unistra.fr>
 *                Finn RAYMENT <rayment@etu.unistra.fr>
 *                Maxime MAIRE <maxime.maire2@etu.unistra.fr
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Monopoly.Menu
{
    public class MenuOptions : MonoBehaviour
    {
        public GameObject OptionsMenu;
        public GameObject OptionsButton;
        public GameObject PauseButton;
        public TMP_Dropdown ResolutionDropdown;
        public TMP_Dropdown QualityDropdown;
        public Slider MusicSlider;
        public Slider SoundSlider;
        public Button Apply;
        public Button Reset;
        public Button Close;
        private Resolution[] AvailableResolutions;
        public static int Resolution;
        public static int Quality;
        public static bool FullScreen;
        public static bool Antialiasing;
        public static bool Shadow;
        public static float Music;
        public static float Sound;

        private bool ChangesApplied;
        void Start()
        {
            ResolutionDropdown.onValueChanged.AddListener
                (delegate { ResolutionChanging();});
            BuildResolutions();
        }
    
    /*
        private void DefaultValues()
        {
            Resolution = 0;
            Quality = 0;
            FullScreen = true;
            Shadow = true;
            Antialiasing = true;
            Music = 0f;
            Sound = 0f;
        }*/
        
        
        private void BuildResolutions()
        {
            AvailableResolutions = Screen.resolutions;
    
            ResolutionDropdown.options.Clear();
            foreach (Resolution Resolution in AvailableResolutions)
            {
                Debug.Log("Resolution " + Resolution.ToString() + " supported.");
                ResolutionDropdown.options.Add(new TMP_Dropdown.OptionData
                    (string.Format("{0}x{1} @ {2}", Resolution.width, 
                        Resolution.height, Resolution.refreshRate)));
            }
            ResolutionDropdown.value = ResolutionDropdown.options.Count - 1;
        }
        public void ResolutionChanging()
        {
            ChangesApplied = false;
            Screen.SetResolution
            (AvailableResolutions[ResolutionDropdown.value].width,
                AvailableResolutions[ResolutionDropdown.value].height, 
                Screen.fullScreen);
        }
    
        public void ResetDefault()
        {
            ChangesApplied = false;
            ResolutionDropdown.value = 0;
            QualityDropdown.value = 0;
            MusicSlider.value = 0f;
            SoundSlider.value = 0f;
        }

        public void ApplyChanges()
        {
            Resolution = ResolutionDropdown.value;
            Quality = QualityDropdown.value;
            Music = MusicSlider.value;
            Sound = SoundSlider.value;
            ChangesApplied = true;
        }

        public void CloseMenu()
        {
            if (!ChangesApplied)
            {
                ResolutionDropdown.value = Resolution;
                QualityDropdown.value = Quality;
                MusicSlider.value = Music;
                SoundSlider.value = Sound;
            }

            OptionsMenu.SetActive(false);
            OptionsButton.SetActive(true);
            PauseButton.SetActive(true);
        }
    }
}

